using CSharpUI.Avalonia.SourceGenerator.ExtensionInfos;

namespace CSharpUI.Avalonia.SourceGenerator.Generators.AttachedPropertySetterGenerator;

public class AttachedPropertyBindFromExpressionSetterGenerator : ExtensionGeneratorBase<AttachedPropertyExtensionInfo>
{
    protected override string? GetExtension(AttachedPropertyExtensionInfo info) => $@"    /// <summary>{info.Comment}</summary>
    public static {info.ReturnType} {info.MemberName}{info.GenericArg}(this {info.ReturnType} control, {info.ValueTypeSource} value){info.GenericConstraint}
    {{
        {info.ControlTypeName}.Set{info.MemberName}(control, value);
        return control;
    }}";
}