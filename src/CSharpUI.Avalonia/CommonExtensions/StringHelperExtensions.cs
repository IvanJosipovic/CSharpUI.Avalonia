using Avalonia.Markup.Xaml.MarkupExtensions;

namespace CSharpUI.Avalonia.CommonExtensions;

/// <summary>
/// String Extensions
/// </summary>
public static class StringHelperExtensions
{
    /// <summary>
    /// Creates a <see cref="DynamicResourceExtension"/> for the given resource key.
    /// </summary>
    /// <param name="dynamicResourceKey"></param>
    /// <returns></returns>
    public static DynamicResourceExtension GetDynamicResource(this string dynamicResourceKey) => new DynamicResourceExtension(dynamicResourceKey);
}