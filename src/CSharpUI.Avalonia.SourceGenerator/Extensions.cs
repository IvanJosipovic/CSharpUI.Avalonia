namespace CSharpUI.Avalonia.SourceGenerator;

internal static class Extensions
{
    internal static string RemoveTrailingProperty(this string source)
    {
        if (source.EndsWith("Property"))
        {
            source = source.Substring(0, source.Length - "Property".Length);
        }

        return source;
    }
}
