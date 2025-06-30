using CSharpUIAvalonia.SourceGenerator.ExtensionInfos;

namespace CSharpUIAvalonia.SourceGenerator.Generators.SetterGenerators;

public class ValueSetterGenerator : ExtensionGeneratorBase<PropertyExtensionInfo>
{
    protected override string? GetExtension(PropertyExtensionInfo info) => $@"    /// <summary>{info.Comment}</summary>
    public static {info.ReturnType} {info.ExtensionName}{info.GenericArg}(this {info.ReturnType} control, {info.ValueTypeSource} value){info.GenericConstraint}
        => control._set(() => control.{info.MemberName} = value!);";
}