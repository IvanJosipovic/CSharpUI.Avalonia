using CSharpUI.Avalonia.SourceGenerator.ExtensionInfos;
using CSharpUI.Avalonia.SourceGenerator.Generators;
using CSharpUI.Avalonia.SourceGenerator.Generators.AttachedPropertySetterGenerator;
using CSharpUI.Avalonia.SourceGenerator.Generators.EventGenerators;
using CSharpUI.Avalonia.SourceGenerator.Generators.SetterGenerators;
using CSharpUI.Avalonia.SourceGenerator.Generators.StyleSetterGenerators;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace CSharpUI.Avalonia.SourceGenerator;

public class GeneratorHost()
{
    readonly List<ExtensionGroupGenerator> groupGenerators =
    [
        new("Properties",
            t => t.GetMembers()
                .OfType<IFieldSymbol>()
                .Where(x => x.IsAvaloniaPropertyField())
                .Select(x => new PropertyExtensionInfo(x)),

            new ValueSetterGenerator(),
            new ValueOverloadsSetterGenerator()
        ),

        new("Attached Properties",
            t => t.GetMembers()
                .OfType<IFieldSymbol>()
                .Where(x => x.IsAttachedPropertyField())
                .Select(x => new AttachedPropertyExtensionInfo(x))
                .Where(x => {

                    var props = t.GetMembers().OfType<IMethodSymbol>();

                    var found = props.FirstOrDefault(y => y.Name.RemoveTrailingProperty() == "Set" + x.MemberName.RemoveTrailingProperty());

                    if (found != null){
                    var paramType = found.Parameters[0].Type;

                    var tName = t.IsOrInheritsFrom(paramType);

                        return tName;
                    }
                    return false;
                }),

            new AttachedPropertyBindFromExpressionSetterGenerator()
        ),

        new("Common Properties",
            t => t.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(x => !x.IsAvaloniaProperty()
                            && x.IsCommonProperty())
                .Select(x => new PropertyExtensionInfo(x)),

            new ValueSetterGenerator()
        ),

        //new("Events",
        //    t => t.GetAllMembers()
        //        .OfType<IEventSymbol>()
        //        .Where(x => x.DeclaredAccessibility == Accessibility.Public
        //                && SymbolEqualityComparer.Default.Equals(x.ContainingType, t))
        //        .Select(x => new EventExtensionInfo(x)),

        //    new ActionToEventGenerator()),

        new("Styles",
            t => !t.IsStyledElement() ? [] : t
                .GetMembers()
                .OfType<IFieldSymbol>()
                .Where(x => x.IsAcceptableStyledField())
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
        sb.AppendLine($"using Avalonia.Data;");
        //sb.AppendLine($"using Avalonia.Data.Converters;");
        //sb.AppendLine($"using System;");
        //sb.AppendLine($"using System.Numerics;");
        //sb.AppendLine($"using System.Linq.Expressions;");
        //sb.AppendLine($"using System.Runtime.CompilerServices;");
        sb.AppendLine("using CSharpUI.Avalonia.Styles;");
        sb.AppendLine("using CSharpUI.Avalonia.CommonExtensions;");
        controlType.GetNamespaces().OrderBy(x => x).ToList().ForEach(x => sb.AppendLine($"using {x};"));
        sb.AppendLine();
        sb.AppendLine("namespace CSharpUI.Avalonia.Extensions;");
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
}