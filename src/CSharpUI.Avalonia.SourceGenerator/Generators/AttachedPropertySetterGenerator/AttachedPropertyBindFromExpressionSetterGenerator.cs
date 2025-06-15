using CSharpUI.Avalonia.SourceGenerator.ExtensionInfos;

namespace CSharpUI.Avalonia.SourceGenerator.Generators.AttachedPropertySetterGenerator;

public class AttachedPropertyBindFromExpressionSetterGenerator : ExtensionGeneratorBase<AttachedPropertyExtensionInfo>
{
    protected override string? GetExtension(AttachedPropertyExtensionInfo info) =>
        $"public static {info.ReturnType} {info.ExtensionName}{info.GenericArg}(this {info.ReturnType} control, Func<{info.ValueTypeSource}> func, Action<{info.ValueTypeSource}>? onChanged = null, [CallerArgumentExpression(nameof(func))] string? expression = null)" +
        $" where T : {info.AttachedPropertyHostTypeName} {Extensions.NewLine}" +
        $"   => control._set({info.ControlTypeName}.{info.FieldInfo.Name}!, func, onChanged, expression);";

}