using Microsoft.CodeAnalysis.CSharp;
using System.Text;

namespace CSharpUI.Avalonia.SourceGenerator;

public class SourceGeneratorBase
{
    private static readonly char[] InvalidHintNameChars =
    [
        '<', '>', ':', '"', '/', '\\', '|', '?', '*'
    ];

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
}
