using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace CSharpUI.Avalonia.SourceGenerator;

public class SourceGeneratorBase
{
    internal const string NewLine = "\r\n";

    private static readonly char[] InvalidHintNameChars =
    [
        '<', '>', ':', '"', '/', '\\', '|', '?', '*'
    ];

    public static void GenerateSource(SourceProductionContext context, INamedTypeSymbol type)
    {
        var typeNamespace = type.ContainingNamespace.ToString();
        var sb = new StringBuilder();

        sb.AppendLine("#nullable enable");
        sb.AppendLine("using System;");
        sb.AppendLine("using Avalonia.Data;");
        sb.AppendLine("using Avalonia.Data.Converters;");
        sb.AppendLine("using System.Runtime.CompilerServices;");

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

        sb.AppendLine();
        sb.AppendLine("namespace CSharpUI.Avalonia;");
        sb.AppendLine();
        sb.AppendLine($"public static partial class {CleanIdentifier(typeName)}Extensions");
        sb.AppendLine("{");

        var members = type.GetMembers();
        var processedFields = new List<string>();

        // PROCESS AVALONIA PROPERTIES
        foreach (var field in members.OfType<IFieldSymbol>())
        {
            if (field.Type is INamedTypeSymbol namedType &&
                namedType.Name is "DirectProperty" or "StyledProperty" or "AttachedProperty" &&
                HasAvaloniaPropertyPublicSetter(field, members))
            {
                //sb.AppendLine($"    // avalonia properties");
                //AppendIfNotNull(sb, GetCommonPropertyExpressionBindingSetterExtension(type, genericParams))
                // AppendIfNotNull(sb, GetPropertySetterExtension(typeName, genericParams, field));
                // AppendIfNotNull(sb, GetExpressionBindingSetterExtension(typeName, genericParams, field));
                //processedFields.Add(field.Name);
            }
        }

        // PROCESS COMMON PROPERTIES
        foreach (var property in members.OfType<IPropertySymbol>())
        {
            if (IsPublic(property) && HasPublicSetter(property)
                //&& IsCommonInstanceProperty(property, members)
                )
            {
                sb.AppendLine($"    // common properties");

                AppendIfNotNull(sb, GetCommonPropertySetterExtension(typeName, genericParams, property));
                //AppendIfNotNull(sb, GetCommonPropertyBindingSetterExtension(typeName, property, semanticModel));
                //AppendIfNotNull(sb, GetCommonPropertyExpressionBindingSetterExtension(typeName, property, semanticModel));

                processedFields.Add(property.Name);
            }
        }

        sb.AppendLine("}");

        if (processedFields.Count > 0)
        {
            context.AddSource($"{RemoveIllegalFileNameCharacters(typeName)}Extensions.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }
    }

    public static string RemoveIllegalFileNameCharacters(string fileName)
    {
        return string.Concat(fileName.Select(c => InvalidHintNameChars.Contains(c) || char.IsControl(c) ? '_' : c));
    }

    public static string? CleanIdentifier(string name, bool @namespace = false)
    {
        // trim off leading and trailing whitespace
        name = name.Trim();

        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        var sb = new StringBuilder();
        if (!SyntaxFacts.IsIdentifierStartCharacter(name[0]))
        {
            // the first characters
            sb.Append('_');
        }

        foreach (var ch in name)
        {
            if (SyntaxFacts.IsIdentifierPartCharacter(ch) || @namespace && ch == '.')
            {
                sb.Append(ch);
            }
        }

        var result = sb.ToString();

        if (SyntaxFacts.GetKeywordKind(result) != SyntaxKind.None)
        {
            result = '@' + result;
        }

        if (@namespace)
        {
            var newResult = string.Empty;
            foreach (var chunk in result.Split('.'))
            {
                if (!string.IsNullOrEmpty(newResult))
                {
                    newResult += '.';
                }

                if (SyntaxFacts.GetKeywordKind(chunk) != SyntaxKind.None)
                {
                    newResult += '@' + chunk;
                }
                else
                {
                    newResult += chunk;
                }
            }

            return newResult;
        }

        return result;
    }

    private static void AppendIfNotNull(StringBuilder sb, string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return;
        sb.AppendLine(value);
    }

    private static bool IsCommonInstanceProperty(IPropertySymbol property, ImmutableArray<ISymbol> members)
    {
        if (property == null || property.IsStatic)
            return false;

        var avaloniaPropertyName = property.Name + "Property";

        return members
            .OfType<IFieldSymbol>()
            .All(field => field.Name != avaloniaPropertyName);
    }

    private static bool IsPublic(IPropertySymbol property)
    {
        return property != null && property.DeclaredAccessibility == Accessibility.Public;
    }

    private static bool HasAvaloniaPropertyPublicSetter(IFieldSymbol field, ImmutableArray<ISymbol> members)
    {
        var backingPropertyName = field.Name;

        if (backingPropertyName.EndsWith("Property"))
        {
            backingPropertyName = backingPropertyName.Substring(0, backingPropertyName.Length - "Property".Length);
        }

        var property = members
            .OfType<IPropertySymbol>()
            .FirstOrDefault(x => x.Name == backingPropertyName);

        return HasPublicSetter(property);
    }

    private static bool HasPublicSetter(IPropertySymbol property)
    {
        if (property == null)
            return false;

        var setter = property.SetMethod;
        if (setter != null && setter.DeclaredAccessibility == Accessibility.Public)
            return true;

        return false;
    }

    private static bool IsAvaloniaPropertyField(IFieldSymbol field)
    {
        if (field.GetAttributes().Any(x => x.AttributeClass?.Name == nameof(ObsoleteAttribute)))
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
        var propertyName = field.Name;

        if (field.AssociatedSymbol != null)
        {
            propertyName = field.AssociatedSymbol.Name;
        }

        if (propertyName.EndsWith("Property"))
        {
            propertyName = propertyName.Substring(0, propertyName.Length - "Property".Length);
        }

        var symbol = field.ContainingType?.GetMembers(propertyName).FirstOrDefault();

        if (symbol is IPropertySymbol prop)
        {
            return !HasPublicSetter(prop);
        }

        return true;
    }

    private static bool IsReadOnlyAttachedField(IFieldSymbol field)
    {
        var controlType = field.ContainingType;
        var setterMethodName = "Set" + field.Name;

        if (setterMethodName.EndsWith("Property"))
        {
            setterMethodName = setterMethodName.Substring(0, setterMethodName.Length - "Property".Length);
        }

        var methodInfo = controlType?.GetMembers(setterMethodName).FirstOrDefault();

        if (methodInfo is IMethodSymbol method)
        {
            return method.DeclaredAccessibility == Accessibility.Public && method.IsStatic;
        }

        return false;
    }

    public static bool IsStyledElement(INamedTypeSymbol controlType)
    {
        return controlType.AllInterfaces.Any(x => x.Name == "StyledElement");
    }

    public static bool IsDeclarativeViewBase(INamedTypeSymbol controlType)
    {
        return controlType.AllInterfaces.Any(x => x.Name == "IDeclarativeViewBase");
    }

    private static string GetCommonPropertySetterExtension(string controlTypeName, string genericParams, IPropertySymbol property)
    {
        var extensionName = property.Name;

        var valueTypeSource = property.Type.Name;

        var argsString = $"{valueTypeSource} value";

        var extensionText =
            $"    public static {controlTypeName} {extensionName}{genericParams}(this {controlTypeName} control, {argsString}) =>{NewLine} "
          + $"        control._set(() => control.{extensionName} = value);";

        return extensionText;
    }

    private static bool IsGenerateExtensionsView(Compilation compilation, ClassDeclarationSyntax component)
    {
        var sModel = compilation.GetSemanticModel(component.SyntaxTree);
        var classSymbol = ModelExtensions.GetDeclaredSymbol(sModel, component);

        return classSymbol is INamedTypeSymbol cs && cs.AllInterfaces.Any(x => x.Name == "IDeclarativeViewBase");
    }

    internal static IEnumerable<INamedTypeSymbol> GetPublicClasses(INamespaceSymbol sym)
    {
        foreach (INamedTypeSymbol typeMember in sym.GetTypeMembers())
        {
            if (typeMember.DeclaredAccessibility == Accessibility.Public && typeMember.TypeKind == TypeKind.Class)
                yield return typeMember;
        }
        foreach (INamespaceSymbol namespaceMember in sym.GetNamespaceMembers())
        {
            foreach (INamedTypeSymbol publicClass in GetPublicClasses(namespaceMember))
            {
                if (publicClass.DeclaredAccessibility == Accessibility.Public && publicClass.TypeKind == TypeKind.Class)
                    yield return publicClass;
            }
        }
    }

    internal static string GetNullableLambdaParameterTypeName(PropertyDeclarationSyntax property, SemanticModel semanticModel)
    {
        var typeInfo = semanticModel.GetTypeInfo(property.Type);
        var typeSymbol = typeInfo.Type;

        if (typeSymbol == null)
            return property.Type + "?";

        var nullableTypeSymbol = typeSymbol.WithNullableAnnotation(NullableAnnotation.Annotated);

        return nullableTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    }
}
