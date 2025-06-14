using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace CSharpUI.Avalonia.SourceGenerator;

[Generator]
public class ExtensionAvaloniaPropertyExtensionsGenerator : SourceGeneratorBase, IIncrementalGenerator
{

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
#if DEBUG
        if (!Debugger.IsAttached)
        {
            //Debugger.Launch();
        }
#endif
        Debug.WriteLine("Initialize ExtensionAvaloniaPropertyExtensionsGenerator code generator");

        var attribute = context.SyntaxProvider
                         .ForAttributeWithMetadataName("CSharpUI.Avalonia.GenerateExtensionsForAssemblyAttribute",
                                                       static (s, _) => true,
                                                       static (ctx, _) => GetSemanticTarget(ctx))
                         .Collect()
                         .SelectMany((x, _) => x.SelectMany(y => y).Distinct(SymbolEqualityComparer.Default));


        context.RegisterSourceOutput(attribute,
            static (spc, data) => GetClasses(spc, data));
    }

    private static ImmutableArray<IAssemblySymbol> GetSemanticTarget(GeneratorAttributeSyntaxContext context)
    {
        var assemblies = ImmutableArray.CreateBuilder<IAssemblySymbol>();

        foreach (var attribute in context.Attributes)
        {
            if (attribute?.AttributeClass?.Name == "GenerateExtensionsForAssemblyAttribute" &&
                attribute.ConstructorArguments.Length > 0 &&
                attribute.ConstructorArguments[0].Value is INamedTypeSymbol iNamedTypeSymbol)
                assemblies.Add(iNamedTypeSymbol.ContainingAssembly);
        }

        return assemblies.ToImmutable();
    }

    private static void GetClasses(SourceProductionContext spc, ISymbol? symbol)
    {
        if (symbol != null && symbol is IAssemblySymbol assembly)
        {
            foreach (INamedTypeSymbol publicClass in assembly.GlobalNamespace.GetPublicClasses())
            {
                GenerateSource(spc, publicClass);
            }
        }
    }

    private static void GenerateSource(SourceProductionContext context, INamedTypeSymbol type)
    {
        //var root = type.SyntaxTree.GetRoot();
        //var ns = root.DescendantNodes()
        //    .FirstOrDefault(x => x is BaseNamespaceDeclarationSyntax) as BaseNamespaceDeclarationSyntax;

        var typeNamespace = type.ContainingNamespace.Name;
        var sb = new StringBuilder();

        sb.AppendLine("#nullable enable");
        sb.AppendLine($"// Auto-generated code {DateTime.Now:g}");
        sb.AppendLine("using System;");
        sb.AppendLine("using Avalonia.Data;");
        sb.AppendLine("using Avalonia.Data.Converters;");
        sb.AppendLine("using System.Runtime.CompilerServices;");

        //if (root is CompilationUnitSyntax compilationUnit)
        //{
        //    foreach (var usingDirective in compilationUnit.Usings)
        //    {
        //        sb.AppendLine(usingDirective.ToString());
        //    }
        //}

        if (!string.IsNullOrWhiteSpace(typeNamespace))
            sb.AppendLine($"using {typeNamespace};");

        var typeName = type.Name;

        var genericParams = "";

        if (type.TypeParameters != null && type.TypeParameters.Any())
        {
            genericParams += '<';

            foreach (var item in type.TypeParameters)
            {
                genericParams += item.Name + ", ";
            }
            genericParams = genericParams.TrimEnd(' ').TrimEnd(',');
            genericParams += '>';
        }
        typeName = typeName + genericParams;

        sb.AppendLine("namespace CSharpUI.Avalonia;");
        sb.AppendLine($"public static partial class {CleanIdentifier(typeName)}Extensions");
        sb.AppendLine("{");

        var members = type.GetMembers();
        var processedFields = new List<string>();

        //// PROCESS AVALONIA PROPERTIES
        foreach (var field in members.OfType<IFieldSymbol>())
        {
            if (IsAvaloniaPropertyField(field))
            {
                sb.AppendLine($"// avalonia properties\n");
                //AppendIfNotNull(sb, GetPropertySetterExtension(typeName, genericParams, field));
                //AppendIfNotNull(sb, GetExpressionBindingSetterExtension(typeName, genericParams, field));
                processedFields.Add(field.Name);
            }
        }

        //// PROCESS COMMON PROPERTIES
        //foreach (var property in members.OfType<PropertyDeclarationSyntax>())
        //{
        //    var propertyName = property.Identifier.ToString();
        //    if (!processedFields.Contains(propertyName + "Property")
        //        && IsPublic(property)
        //        && HasPublicSetter(property)
        //        && IsCommonInstanceProperty(property, members))
        //    {
        //        sb.AppendLine($"// common properties\n");

        //        //AppendIfNotNull(sb, GetCommonPropertySetterExtension(typeName, property, semanticModel));
        //        //AppendIfNotNull(sb, GetCommonPropertyBindingSetterExtension(typeName, property, semanticModel));
        //        //AppendIfNotNull(sb, GetCommonPropertyExpressionBindingSetterExtension(typeName, property, semanticModel));

        //        processedFields.Add(propertyName);
        //    }
        //}

        sb.AppendLine("}");

        if (processedFields.Count > 0)
        {
            context.AddSource($"{RemoveIllegalFileNameCharacters(typeName)}Extensions.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }
    }

    private static bool IsAvaloniaPropertyField(IFieldSymbol field)
    {
        if (field.GetAttributes().Any(x => x.AttributeClass?.Name == "ObsoleteAttribute"))
            return false;

        if (field.Type.Name.StartsWith("DirectProperty") ||
            field.Type.Name.StartsWith("StyledProperty") ||
            //some attached properties Mapped to properties of controls, i.e. TextBlock.TextWrapping
            //so we need to add direct Extensions for them, additionally to AttachedProperty extensions
            field.Type.Name.StartsWith("AttachedProperty") ||
            field.Type.Name.StartsWith("AvaloniaProperty"))
        {
            return !IsReadOnlyField(field);
        }

        return false;
    }

    private static bool IsReadOnlyField(IFieldSymbol field)
    {
        var controlType = field.ContainingType;
        var propertyName = field.Name.Replace("Property", "");

        if (field.AssociatedSymbol != null)
        {
            propertyName = field.AssociatedSymbol.Name.Replace("Property", "");
        }

        var symbol = controlType?.GetMembers(propertyName).FirstOrDefault();

        if (symbol is IPropertySymbol prop)
        {
            return !prop.HasPublicSetter();
        }

        return true;
    }

    private static bool IsReadOnlyAttachedField(IFieldSymbol field)
    {
        var controlType = field.ContainingType;
        var setterMethodName = "Set" + field.Name.Replace("Property", "");

        var methodInfo = controlType?.GetMembers(setterMethodName).FirstOrDefault();

        if (methodInfo is IMethodSymbol method)
        {
            return method.DeclaredAccessibility == Accessibility.Public && method.IsStatic;
        }

        return false;
    }

    public static bool IsStyledElement(INamedTypeSymbol controlType)
    {
        return controlType.AllInterfaces.Any(x => x.Name == "IStyleable");
    }

    public static bool IsDeclarativeViewBase(INamedTypeSymbol controlType)
    {
        return controlType.AllInterfaces.Any(x => x.Name == "IDeclarativeViewBase");
    }
}