using CSharpUI.Avalonia.SourceGenerator.ExtensionInfos;

namespace CSharpUI.Avalonia.SourceGenerator.Generators.SetterGenerators;

public class AvaloniaPropertyBindSetterGenerator : ExtensionGeneratorBase<PropertyExtensionInfo>
{
    protected override string? GetExtension(PropertyExtensionInfo info)=>
        $"public static {info.ReturnType} {info.ExtensionName}{info.GenericArg}(this {info.ReturnType} control, AvaloniaProperty avaloniaProperty, BindingMode? bindingMode = null, IValueConverter? converter = null, ViewBase? overrideView = null) {info.GenericConstraint}{Extensions.NewLine}" +
        $"   => control._set({info.ControlTypeName}.{info.FieldInfo.Name}, avaloniaProperty, bindingMode, converter, overrideView);";

}