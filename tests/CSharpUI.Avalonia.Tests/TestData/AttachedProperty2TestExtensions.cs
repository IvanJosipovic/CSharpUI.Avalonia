#nullable enable

namespace CSharpUI.Avalonia.Extensions;

public static partial class AttachedProperty2TestExtensions
{
    //================= Styles ======================//
    // LineSpacing

    /*ValueStyleSetterGenerator*/
    /// <summary></summary>
    public static global::CSharpUI.Avalonia.Styles.Style<T> LineSpacing<T>(this global::CSharpUI.Avalonia.Styles.Style<T> style, global::System.Double value) where T : global::Tests.AttachedProperty2Test
        => style._addSetter(global::Tests.AttachedProperty2Test.LineSpacingProperty!, value!);

    /*BindingStyleSetterGenerator*/
    /// <summary></summary>
    public static global::CSharpUI.Avalonia.Styles.Style<T> LineSpacing<T>(this global::CSharpUI.Avalonia.Styles.Style<T> style, global::Avalonia.Data.IBinding binding) where T : global::Tests.AttachedProperty2Test
        => style._addSetter(global::Tests.AttachedProperty2Test.LineSpacingProperty, binding);



}