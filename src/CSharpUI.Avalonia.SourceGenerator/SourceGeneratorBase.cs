using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace CSharpUI.Avalonia.SourceGenerator;

public class SourceGeneratorBase
{
    internal const string NewLine = "\r\n";

    private static readonly char[] InvalidHintNameChars =
    [
        '<', '>', ':', '"', '/', '\\', '|', '?', '*'
    ];

    public static void GenerateSource(SourceProductionContext context, INamedTypeSymbol type)
    {
        var typeNamespace = type.ContainingNamespace.ToString();
        var sb = new StringBuilder();

        sb.AppendLine("#nullable enable");
        sb.AppendLine("using System;");
        sb.AppendLine("using Avalonia.Data;");
        sb.AppendLine("using Avalonia.Data.Converters;");
        sb.AppendLine("using System.Runtime.CompilerServices;");

        if (!string.IsNullOrWhiteSpace(typeNamespace))
            sb.AppendLine($"using {typeNamespace};");

        var typeName = type.Name;

        var genericParams = "";

        if (type.TypeParameters != null && type.TypeParameters.Any())
        {
            genericParams += '<';

            foreach (var item in type.TypeParameters)
            {
                genericParams += item.Name + ", ";
            }
            genericParams = genericParams.TrimEnd(' ').TrimEnd(',');
            genericParams += '>';
        }
        typeName = typeName + genericParams;

        sb.AppendLine();
        sb.AppendLine("namespace CSharpUI.Avalonia;");
        sb.AppendLine();
        sb.AppendLine($"public static partial class {CleanIdentifier(typeName)}Extensions");
        sb.AppendLine("{");

        var members = type.GetMembers();
        var processedFields = new List<string>();

        // PROCESS AVALONIA PROPERTIES
        foreach (var field in members.OfType<IFieldSymbol>())
        {
            if (field.Type is INamedTypeSymbol namedType
                && namedType.Name is "DirectProperty" or "StyledProperty"
                && HasAvaloniaPropertyPublicSetter(field)
                && !IsReadOnlyField(field))
            {
                sb.AppendLine($"    // Avalonia Property: {field.Name}");

                AppendIfNotNull(sb, GetPropertySetterExtension(typeName, genericParams, field));
                // AppendIfNotNull(sb, GetCommonPropertyExpressionBindingSetterExtension(type, genericParams))
                // AppendIfNotNull(sb, GetPropertySetterExtension(typeName, genericParams, field));
                // AppendIfNotNull(sb, GetExpressionBindingSetterExtension(typeName, genericParams, field));
                processedFields.Add(field.Name);
            }
        }

        // PROCESS AVALONIA ATTACHED PROPERTIES
        foreach (var field in members.OfType<IFieldSymbol>())
        {
            if (field.Type is INamedTypeSymbol namedType
                && IsAttachedPropertyField(field))
            {
                sb.AppendLine($"    // Avalonia Attached Property: {field.Name}");

                AppendIfNotNull(sb, GetAttachedPropertySetterExtension(typeName, genericParams, field));
                processedFields.Add(field.Name);
            }
        }

        // PROCESS COMMON PROPERTIES
        foreach (var property in members.OfType<IPropertySymbol>())
        {
            if (IsPublic(property)
             && HasPublicSetter(property)
             && IsCommonInstanceProperty(property))
            {
                sb.AppendLine($"    // Common Property: {property.Name}");

                AppendIfNotNull(sb, GetPropertySetterExtension(typeName, genericParams, property));
                //AppendIfNotNull(sb, GetCommonPropertyBindingSetterExtension(typeName, property, semanticModel));
                //AppendIfNotNull(sb, GetCommonPropertyExpressionBindingSetterExtension(typeName, property, semanticModel));

                processedFields.Add(property.Name);
            }
        }

        sb.AppendLine("}");

        if (processedFields.Count > 0)
        {
            context.AddSource($"{RemoveIllegalFileNameCharacters(type.ConstructedFrom.ToString())}.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }
    }

    public static string RemoveIllegalFileNameCharacters(string fileName)
    {
        return string.Concat(fileName.Select(c => InvalidHintNameChars.Contains(c) || char.IsControl(c) ? '_' : c));
    }

    public static string? CleanIdentifier(string name, bool @namespace = false)
    {
        // trim off leading and trailing whitespace
        name = name.Trim();

        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        var sb = new StringBuilder();
        if (!SyntaxFacts.IsIdentifierStartCharacter(name[0]))
        {
            // the first characters
            sb.Append('_');
        }

        foreach (var ch in name)
        {
            if (SyntaxFacts.IsIdentifierPartCharacter(ch) || @namespace && ch == '.')
            {
                sb.Append(ch);
            }
        }

        var result = sb.ToString();

        if (SyntaxFacts.GetKeywordKind(result) != SyntaxKind.None)
        {
            result = '@' + result;
        }

        if (@namespace)
        {
            var newResult = string.Empty;
            foreach (var chunk in result.Split('.'))
            {
                if (!string.IsNullOrEmpty(newResult))
                {
                    newResult += '.';
                }

                if (SyntaxFacts.GetKeywordKind(chunk) != SyntaxKind.None)
                {
                    newResult += '@' + chunk;
                }
                else
                {
                    newResult += chunk;
                }
            }

            return newResult;
        }

        return result;
    }

    private static void AppendIfNotNull(StringBuilder sb, string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return;
        sb.AppendLine(value);
    }

    #region Property
    private static bool IsCommonInstanceProperty(IPropertySymbol property)
    {
        if (property == null || property.IsStatic)
            return false;

        var avaloniaPropertyName = property.Name + "Property";

        return property.ContainingType.GetMembers()
            .OfType<IFieldSymbol>()
            .All(field => field.Name != avaloniaPropertyName);
    }

    private static bool IsPublic(IPropertySymbol property)
    {
        return property != null && property.DeclaredAccessibility == Accessibility.Public;
    }

    private static bool HasPublicSetter(IPropertySymbol property)
    {
        if (property == null)
            return false;

        var setter = property.SetMethod;
        if (setter != null && setter.DeclaredAccessibility == Accessibility.Public)
            return true;

        return false;
    }

    private static string GetPropertySetterExtension(string controlTypeName, string genericParams, IPropertySymbol property)
    {
        var extensionName = property.Name;

        var argsString = $"{property.Type.Name} value";

        var extensionText =
            $"    public static {controlTypeName} {extensionName}{genericParams}(this {controlTypeName} control, {argsString}) =>{NewLine}"
          + $"        control._set(() => control.{extensionName} = value);";

        return extensionText;
    }

    private static bool IsAvaloniaPropertyField(IPropertySymbol property)
    {
        var field = property.ContainingType.GetMembers().OfType<IFieldSymbol>().FirstOrDefault(x => x.Name == property.Name + "Property");

        if (field == null || field.GetAttributes().Any(x => x.AttributeClass?.Name == "ObsoleteAttribute"))
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

    #endregion

    #region Field
    private static bool IsAttachedPropertyField(IFieldSymbol field)
    {
        if (field.GetAttributes().Any(x => x.AttributeClass?.Name == "ObsoleteAttribute"))
            return false;

        if (field.Type.Name.StartsWith("AttachedProperty"))
        {
            return !IsReadOnlyAttachedField(field);
        }

        return false;
    }

    private static bool HasAvaloniaPropertyPublicSetter(IFieldSymbol field)
    {
        var backingPropertyName = field.Name.RemoveTrailingProperty();

        var property = field.ContainingType.GetMembers()
            .OfType<IPropertySymbol>()
            .FirstOrDefault(x => x.Name == backingPropertyName);

        return HasPublicSetter(property);
    }

    private static bool IsReadOnlyField(IFieldSymbol field)
    {
        var propertyName = field.Name;

        if (field.AssociatedSymbol != null)
        {
            propertyName = field.AssociatedSymbol.Name;
        }

        propertyName = propertyName.RemoveTrailingProperty();

        var symbol = field.ContainingType?.GetMembers(propertyName).FirstOrDefault();

        if (symbol is IPropertySymbol prop)
        {
            return !HasPublicSetter(prop);
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
            return !(method.DeclaredAccessibility == Accessibility.Public && method.IsStatic);
        }

        return true;
    }

    private static string GetPropertySetterExtension(string controlTypeName, string genericParams, IFieldSymbol field)
    {
        var extensionName = field.Name.RemoveTrailingProperty();

        var returnType = "";

        if (field.Type is INamedTypeSymbol nts)
        {
            returnType = nts.TypeArguments.Last().ToString();
        }
        else
        {
            throw new Exception("Unkown Type");
        }

        var argsString = $"{returnType} value";

        var extensionText =
            $"    public static {controlTypeName} {extensionName}{genericParams}(this {controlTypeName} control, {argsString}) =>{NewLine}"
          + $"        control._set(() => control.{extensionName} = value);";

        return extensionText;
    }

    private static string GetAttachedPropertySetterExtension(string controlTypeName, string genericParams, IFieldSymbol field)
    {
        var extensionName = field.Name.RemoveTrailingProperty();

        var returnType = "";

        if (field.Type is INamedTypeSymbol nts)
        {
            returnType = nts.TypeArguments.Last().ToString();
        }
        else
        {
            throw new Exception("Unknown Type");
        }

        var argsString = $"{returnType} value";

        var extensionText =
            $"    public static {controlTypeName} {extensionName}{genericParams}(this {controlTypeName} control, {argsString}) =>{NewLine}"
          + $"        control._set(() => {controlTypeName}.Set{extensionName}(control, value));";

        return extensionText;
    }



    #endregion

    public static bool IsStyledElement(INamedTypeSymbol controlType)
    {
        return controlType.AllInterfaces.Any(x => x.Name == "StyledElement");
    }

    internal static IEnumerable<INamedTypeSymbol> GetPublicClasses(INamespaceSymbol sym)
    {
        foreach (INamedTypeSymbol typeMember in sym.GetTypeMembers())
        {
            if (typeMember.DeclaredAccessibility == Accessibility.Public && typeMember.TypeKind == TypeKind.Class)
                yield return typeMember;
        }
        foreach (INamespaceSymbol namespaceMember in sym.GetNamespaceMembers())
        {
            foreach (INamedTypeSymbol publicClass in GetPublicClasses(namespaceMember))
            {
                if (publicClass.DeclaredAccessibility == Accessibility.Public && publicClass.TypeKind == TypeKind.Class)
                    yield return publicClass;
            }
        }
    }

    internal static bool InheritsFrom(INamedTypeSymbol type, string baseType)
    {
        while (type.BaseType != null)
        {
            if (type.BaseType.ToString() == baseType)
                return true;

            type = type.BaseType;
        }

        return false;
    }
}
