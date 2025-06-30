using Avalonia.Markup.Xaml.MarkupExtensions;

namespace CSharpUIAvalonia.CommonExtensions;

public static class StringHelperExtensions
{
    public static DynamicResourceExtension GetDynamicResource(this string dynamcResourceKey) => new DynamicResourceExtension(dynamcResourceKey);
}