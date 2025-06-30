using CSharpUIAvalonia.SourceGenerator.Generators;
using Microsoft.CodeAnalysis;

namespace CSharpUIAvalonia.SourceGenerator.ExtensionInfos;
public class PropertyExtensionInfo : IMemberExtensionInfo
{
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
        ControlType = field.ContainingType ?? throw new NullReferenceException("Control type can not be NULL");
        ExtensionName = field.Name.RemoveTrailingProperty();
        MemberName = field.Name.RemoveTrailingProperty();
        Comment = field.GetDocumentation();

        if (field.AssociatedSymbol != null)
        {
            ExtensionName = field.AssociatedSymbol.Name.RemoveTrailingProperty();
            MemberName = field.AssociatedSymbol.Name.RemoveTrailingProperty();
        }

        ValueType = field.Type;
        ControlTypeName = ControlType.Name;

        if (ControlType is INamedTypeSymbol ct)
        {
            if (ct.TypeArguments.Length > 0)
            {
                ControlTypeName += "<";
                ControlTypeName += ct.TypeArguments.Select(x => x.Name + (x.NullableAnnotation == NullableAnnotation.Annotated ? "?" : "")).Aggregate((x, y) => x + ", " + y);
                ControlTypeName += ">";
                ValueTypeSource += ct.NullableAnnotation == NullableAnnotation.Annotated ? "?" : "";
            }
        }

        if (field.Type is INamedTypeSymbol nts)
        {
            ValueTypeSource = nts.Name;

            var type = nts.TypeArguments.LastOrDefault();

            if (type != null)
            {
                ValueTypeSource = type.Name;

                if (type is INamedTypeSymbol nts2)
                {
                    if (nts2.TypeArguments.Length > 0)
                    {
                        ValueTypeSource += "<";
                        ValueTypeSource += nts2.TypeArguments.Select(x => x.Name + (x.NullableAnnotation == NullableAnnotation.Annotated ? "?" : "")).Aggregate((x, y) => x + ", " + y);
                        ValueTypeSource += ">";
                        ValueTypeSource += nts2.NullableAnnotation == NullableAnnotation.Annotated ? "?" : "";
                    }
                }
            }
        }
        else if (field.Type is ITypeParameterSymbol tps)
        {
            ValueTypeSource = tps.Name;
        }
        else
        {
            ValueTypeSource = "";
            throw new Exception("shouldnt be here");
        }

        IsAttachedProperty = field.Type.Name.StartsWith("AttachedProperty");
        IsGeneric = !field.Type.IsSealed;

        ReturnType = ControlTypeName;
        if (IsGeneric)
        {
            ReturnType = "T";
            GenericConstraint = $" where T : {ControlTypeName}";
            GenericArg = "<T>";

            if (ControlType is INamedTypeSymbol ct2)
            {
                if (ct2.TypeArguments.Length > 0)
                {
                    GenericConstraint += " " + ct2.TypeArguments.Select(x => $"where {x.Name} : class").Aggregate((x, y) => x + ", " + y);
                    GenericArg = "<T, " + ct2.TypeArguments.Select(x => x.Name).Aggregate((x, y) => x + ", " + y) + ">";
                }
            }
        }
    }

    public PropertyExtensionInfo(IPropertySymbol property)
    {
        ControlType = property.ContainingType ?? throw new NullReferenceException("Control type can not be NULL");
        ExtensionName = property.Name.RemoveTrailingProperty();
        MemberName = property.Name.RemoveTrailingProperty();
        Comment = property.GetDocumentation();

        ValueType = property.Type;
        ControlTypeName = ControlType.Name;

        if (ControlType is INamedTypeSymbol ct)
        {
            if (ct.TypeArguments.Length > 0)
            {
                ControlTypeName += "<";
                ControlTypeName += ct.TypeArguments.Select(x => x.Name + (x.NullableAnnotation == NullableAnnotation.Annotated ? "?" : "")).Aggregate((x, y) => x + ", " + y);
                ControlTypeName += ">";
                ValueTypeSource += ct.NullableAnnotation == NullableAnnotation.Annotated ? "?" : "";
            }
        }

        if (property.Type is INamedTypeSymbol nts)
        {
            ValueTypeSource = nts.Name;

            var type = nts.TypeArguments.LastOrDefault();

            if (type != null)
            {
                ValueTypeSource = type.Name;

                if (type is INamedTypeSymbol nts2)
                {
                    if (nts2.TypeArguments.Length > 0)
                    {
                        ValueTypeSource += "<";
                        ValueTypeSource += nts2.TypeArguments.Select(x => x.Name + (x.NullableAnnotation == NullableAnnotation.Annotated ? "?" : "")).Aggregate((x, y) => x + ", " + y);
                        ValueTypeSource += ">";
                        ValueTypeSource += nts2.NullableAnnotation == NullableAnnotation.Annotated ? "?" : "";
                    }
                }
            }
        }
        else if (property.Type is ITypeParameterSymbol tps)
        {
            ValueTypeSource = tps.Name;
        }
        else
        {
            ValueTypeSource = "";
            throw new Exception("shouldnt be here");
        }

        //IsAttachedProperty = property.Type.Name.StartsWith("AttachedProperty");
        IsGeneric = !property.Type.IsSealed;

        ReturnType = ControlTypeName;
        if (IsGeneric)
        {
            ReturnType = "T";
            GenericConstraint = $" where T : {ControlTypeName}";
            GenericArg = "<T>";

            if (ControlType is INamedTypeSymbol ct2 )
            {
                if (ct2.TypeArguments.Length > 0)
                {
                    GenericConstraint += " " + ct2.TypeArguments.Select(x => $"where {x.Name} : class").Aggregate((x, y) => x + ", " + y);
                    GenericArg = "<T, " + ct2.TypeArguments.Select(x => x.Name).Aggregate((x, y) => x + ", " + y) + ">";
                }
            }
        }
    }
}