using CSharpUI.Avalonia.SourceGenerator.Generators;
using Microsoft.CodeAnalysis;

namespace CSharpUI.Avalonia.SourceGenerator.ExtensionInfos;
public class PropertyExtensionInfo : IMemberExtensionInfo
{
    public IFieldSymbol FieldInfo { get; }
    public ITypeSymbol ControlType { get; }
    public string ControlTypeName { get; }
    public string ExtensionName { get; protected set; }
    public string MemberName { get; }
    public ITypeSymbol ValueType { get; }
    public object ValueTypeSource { get; }
    public bool IsGeneric { get; }
    public bool IsAttachedProperty { get; set; }
    public string ReturnType { get; set; }
    public string GenericConstraint { get; set; } = "";
    public string GenericArg { get; set; } = "";
    public string? Comment { get; set; }


    public PropertyExtensionInfo(IFieldSymbol field)
    {
        FieldInfo = field;
        ControlType = field.ContainingType ?? throw new NullReferenceException("Control type can not be NULL");
        ExtensionName = field.Name.RemoveTrailingProperty();
        MemberName = field.Name.RemoveTrailingProperty();
        Comment = Extensions.GetDocumentation(field);

        if (field.AssociatedSymbol != null)
        {
            ExtensionName = field.AssociatedSymbol.Name.RemoveTrailingProperty();
            MemberName = field.AssociatedSymbol.Name.RemoveTrailingProperty();
        }

        ValueType = field.Type;
        ControlTypeName = ControlType.Name;

        var t = (INamedTypeSymbol)field.Type;

        var type = t.TypeArguments.LastOrDefault();

        type ??= t;

        ValueTypeSource = type.Name;

        IsAttachedProperty = field.Type.Name.StartsWith("AttachedProperty");
        IsGeneric = !field.Type.IsSealed;

        ReturnType = ControlTypeName;
        if (IsGeneric)
        {
            ReturnType = "T";
            GenericConstraint = $"where T : {ControlTypeName}";
            GenericArg = "<T>";
        }
    }

    public static IMemberExtensionInfo Create(IFieldSymbol fieldInfo)
    {
        return new PropertyExtensionInfo(fieldInfo);
    }
}