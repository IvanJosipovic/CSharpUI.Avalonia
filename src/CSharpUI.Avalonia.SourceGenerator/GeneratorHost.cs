using System.Collections.Immutable;
using System.Text;
using CSharpUI.Avalonia.SourceGenerator.ExtensionInfos;
using CSharpUI.Avalonia.SourceGenerator.Generators;
using CSharpUI.Avalonia.SourceGenerator.Generators.AttachedPropertySetterGenerator;
using CSharpUI.Avalonia.SourceGenerator.Generators.EventGenerators;
using CSharpUI.Avalonia.SourceGenerator.Generators.SetterGenerators;
using CSharpUI.Avalonia.SourceGenerator.Generators.StyleSetterGenerators;
using Microsoft.CodeAnalysis;

namespace CSharpUI.Avalonia.SourceGenerator;

public class GeneratorHost()
{
    readonly List<ExtensionGroupGenerator> groupGenerators =
    [
        new("Properties",
            t => t.GetMembers()
                .OfType<IFieldSymbol>()
                .Where(x => IsAvaloniaPropertyField(x))
                .Select(x => new PropertyExtensionInfo(x)),

            new ValueSetterGenerator(),
            new ValueOverloadsSetterGenerator()
        ),

        new("Attached Properties",
            t => t.GetMembers()
                .OfType<IFieldSymbol>()
                .Where(IsAttachedPropertyField)
                .Select(x => new AttachedPropertyExtensionInfo(x)),

            new AttachedPropertyBindFromExpressionSetterGenerator()
        ),

        new("Common Properties",
            t => t.GetMembers()
                .OfType<IFieldSymbol>()
                .Where(x => !IsAvaloniaPropertyField(x) && IsCommonPropertyField(x))
                .Select(x => new PropertyExtensionInfo(x)),

            new ValueSetterGenerator()
        ),

        new("Events",
            t => t.GetMembers()
                .OfType<IEventSymbol>()
                .Where(x => SymbolEqualityComparer.Default.Equals(x.ContainingType, t))
                .Select(x => new EventExtensionInfo(x)),

            new ActionToEventGenerator()),

        new("Styles",
            t => !IsStyledElement(t) ? [] : t
                .GetMembers()
                .OfType<IFieldSymbol>()
                .Where(IsAcceptableStyledField)
                .Select(x => new PropertyExtensionInfo(x)),

            new ValueStyleSetterGenerator(),
            new BindingStyleSetterGenerator(),
            new ValueOverloadsStyleSetterGenerator()
        )
    ];

    public string? GenerateExtensions(INamedTypeSymbol controlType)
    {
        var extensionGroups = groupGenerators.Select(x =>
        {
            var extensions = x.Generate(controlType, out var generationsCount);
            return (x.GroupName, extensions, amount: generationsCount);
        }).ToImmutableList();

        //skip types without extensions
        if (extensionGroups.All(x => x.amount == 0))
            return null;

        var sb = new StringBuilder();
        sb.AppendLine("#nullable enable");
        //sb.AppendLine($"using Avalonia.Data;");
        //sb.AppendLine($"using Avalonia.Data.Converters;");
        //sb.AppendLine($"using System;");
        //sb.AppendLine($"using System.Numerics;");
        //sb.AppendLine($"using System.Linq.Expressions;");
        //sb.AppendLine($"using System.Runtime.CompilerServices;");
        sb.Append("using CSharpUI.Avalonia.Styles;");
        sb.Append("using CSharpUI.Avalonia.CommonExtensions;");
        GetNamespaces(controlType).OrderBy(x => x).ToList().ForEach(x => sb.AppendLine($"using {x};"));
        sb.AppendLine();
        sb.AppendLine("namespace CSharpUI.Avalonia;");
        sb.AppendLine();
        sb.AppendLine($"public static partial class {Extensions.CleanIdentifier(controlType.Name)}Extensions");
        sb.AppendLine("{");

        foreach (var group in extensionGroups.Where(x => x.extensions != null))
        {
            sb.AppendLine($"    //================= {group.GroupName} ======================//");
            sb.AppendLine(group.extensions);
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static List<string> GetNamespaces(INamedTypeSymbol type)
    {
        var namespaces = new HashSet<string>();

        void AddNamespace(INamespaceSymbol? ns)
        {
            if (ns == null || ns.IsGlobalNamespace) return;
            namespaces.Add(ns.ToDisplayString());
        }

        void CollectFromType(ITypeSymbol? t)
        {
            if (t == null) return;
            if (t is INamedTypeSymbol named)
            {
                AddNamespace(named.ContainingNamespace);
                foreach (var arg in named.TypeArguments)
                    CollectFromType(arg);
            }
            else if (t is IArrayTypeSymbol arr)
            {
                CollectFromType(arr.ElementType);
            }
            else if (t is IPointerTypeSymbol ptr)
            {
                CollectFromType(ptr.PointedAtType);
            }
        }

        foreach (var member in type.GetMembers())
        {
            switch (member)
            {
                case IPropertySymbol prop:
                    CollectFromType(prop.Type);
                    foreach (var param in prop.Parameters)
                        CollectFromType(param.Type);
                    break;
                case IMethodSymbol method:
                    CollectFromType(method.ReturnType);
                    foreach (var param in method.Parameters)
                        CollectFromType(param.Type);
                    break;
                case IFieldSymbol field:
                    CollectFromType(field.Type);
                    break;
                case IEventSymbol evt:
                    CollectFromType(evt.Type);
                    break;
            }
        }

        // Also add the type's own namespace
        AddNamespace(type.ContainingNamespace);

        return [..namespaces];
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

    private static bool IsCommonPropertyField(IFieldSymbol field)
    {
        if (field.GetAttributes().Any(x => x.AttributeClass?.Name == "ObsoleteAttribute"))
            return false;

        return !IsReadOnlyField(field);
    }

    private static bool IsAttachedPropertyField(IFieldSymbol field)
    {
        if (field.GetAttributes().Any(x => x.AttributeClass?.Name == "ObsoleteAttribute"))
            return false;

        if (field.Type.Name.StartsWith("AttachedProperty"))
        {
            //Console.ForegroundColor = ConsoleColor.Magenta;
            //Console.WriteLine($"{field.Name} is Attached Property.");

            var isReadOnly = IsReadOnlyAttachedField(field);
            if (isReadOnly)
            {
                //Console.ForegroundColor = ConsoleColor.Cyan;
                //Console.WriteLine($"{field.Name} is read only - skipped.");
                //Console.ForegroundColor = ConsoleColor.Gray;
                return false;
            }

            //Console.ForegroundColor = ConsoleColor.Gray;
            return true;
        }
        return false;
    }

    private static bool IsAcceptableStyledField(IFieldSymbol field)
    {
        if (field.GetAttributes().Any(x => x.AttributeClass?.Name == "ObsoleteAttribute"))
            return false;

        if (field.Type.Name.StartsWith("StyledProperty") ||
            field.Type.Name.StartsWith("AttachedProperty"))
            return !IsReadOnlyField(field);

        return false;
    }

    private static bool IsReadOnlyField(IFieldSymbol field)
    {
        var controlType = field.ContainingType;
        var propertyName = field.Name.RemoveTrailingProperty();

        if (field.AssociatedSymbol != null)
        {
            propertyName = field.AssociatedSymbol.Name.RemoveTrailingProperty();
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
        var setterMethodName = "Set" + field.Name.RemoveTrailingProperty();

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
}