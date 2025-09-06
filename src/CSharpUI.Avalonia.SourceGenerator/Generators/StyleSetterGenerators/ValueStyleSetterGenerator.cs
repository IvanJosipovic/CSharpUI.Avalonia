using CSharpUI.Avalonia.SourceGenerator.ExtensionInfos;

namespace CSharpUI.Avalonia.SourceGenerator.Generators.StyleSetterGenerators;

public class ValueStyleSetterGenerator : ExtensionGeneratorBase<PropertyExtensionInfo>
{
    protected override string? GetExtension(PropertyExtensionInfo info)
    {
        //    public static Style<TElement> Background<TElement>(this Style<TElement> style, Brush brush) where TElement : TemplatedControl =>
        //        AddSetter(style, new Setter(TemplatedControl.BackgroundProperty, brush));

        if (info.ValueTypeSource.ToString() == "IBinding")
        {
            return "// Skipped as return type is IBinding";
        }

        //direct type access
        var extensionText =
            $"    public static Style<{info.ReturnType}> {info.ExtensionName}{info.GenericArg}(this Style<{info.ReturnType}> style, {info.ValueTypeSource} value){info.GenericConstraint}{Extensions.NewLine}"
          + $"        => style._addSetter({info.ControlTypeName}.{info.MemberName}Property!, value!);";

        return extensionText;
    }
}