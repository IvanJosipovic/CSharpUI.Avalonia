using Microsoft.CodeAnalysis;

namespace CSharpUI.Avalonia.SourceGenerator.ExtensionInfos;

public class AttachedPropertyExtensionInfo : PropertyExtensionInfo
{
    public string? AttachedPropertyHostTypeName { get; set; }

    public AttachedPropertyExtensionInfo(IFieldSymbol field) : base(field)
    {
        ExtensionName = $"{ControlType.Name}_{ExtensionName}";
        var declaredType = field.Type;

        var symbol = declaredType.GetMembers().FirstOrDefault(x => x.Name == $"Set{MemberName}");

        if (symbol is IMethodSymbol method)
        {
            var par = method.Parameters.FirstOrDefault();
            if (par != null)
            {
                AttachedPropertyHostTypeName = par.Type.ToString();
            }
        }
    }
}