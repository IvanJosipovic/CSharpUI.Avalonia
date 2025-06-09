using Avalonia.Markup.Xaml.MarkupExtensions;

namespace CSharpUI.Avalonia.CommonExtensions;

public static class StringHelperExtensions
{
    public static DynamicResourceExtension GetDynamicResource(this string dynamcResourceKey) => new DynamicResourceExtension(dynamcResourceKey);
}