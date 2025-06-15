using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace CSharpUI.Avalonia.SourceGenerator.Generators;

internal static class Extensions
{
    public static string NewLine = "\r\n";

    private static readonly char[] InvalidHintNameChars =
    [
        '<', '>', ':', '"', '/', '\\', '|', '?', '*'
    ];

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

    public static string RemoveIllegalFileNameCharacters(string fileName)
    {
        return string.Concat(fileName.Select(c => InvalidHintNameChars.Contains(c) || char.IsControl(c) ? '_' : c));
    }

    internal static string RemoveTrailingProperty(this string source)
    {
        if (source.EndsWith("Property"))
        {
            source = source.Substring(0, source.Length - "Property".Length);
        }

        return source;
    }

    internal static bool HasPublicSetter(this IPropertySymbol? property)
    {
        return property?.SetMethod != null && property.SetMethod.DeclaredAccessibility == Accessibility.Public;
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

    internal static string? GetDocumentation(IFieldSymbol field)
    {
        var docs = field.GetDocumentationCommentXml();

        if (!string.IsNullOrEmpty(docs))
        {
            docs = GetSummary(docs!);
        }

        if (string.IsNullOrEmpty(docs))
        {
            var propertyName = field.Name.RemoveTrailingProperty();

            if (field.AssociatedSymbol != null)
            {
                propertyName = field.AssociatedSymbol!.MetadataName;
            }

            var property = field.ContainingType
                .GetMembers()
                .OfType<IPropertySymbol>()
                .FirstOrDefault(p => p.Name == propertyName);

            if (property != null)
            {
                docs = property.GetDocumentationCommentXml();
                if (!string.IsNullOrEmpty(docs))
                {
                    docs = GetSummary(docs!);
                }
            }
        }

        static string? GetSummary(string comment)
        {
            try
            {
                var element = XElement.Parse(comment);
                var summary = element.Element("summary")?.Value.Trim();
                return summary;
            }
            catch (Exception)
            {
            }
            return null;
        }

        return docs;
    }
}
