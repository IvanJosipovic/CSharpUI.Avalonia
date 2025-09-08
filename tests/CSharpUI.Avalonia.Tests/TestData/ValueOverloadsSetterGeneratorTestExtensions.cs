#nullable enable

namespace CSharpUI.Avalonia.Extensions;

public static partial class ValueOverloadsSetterGeneratorTestExtensions
{
    //================= Properties ======================//
    // Padding

    /*ValueSetterGenerator*/
    /// <summary>Gets or sets the padding to place around the .</summary>
    public static T Padding<T>(this T control, global::Avalonia.Thickness value) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => control._set(() => control.Padding = value!);

    /*ValueOverloadsSetterGenerator*/
    /// <summary>Gets or sets the padding to place around the .</summary>
    public static T Padding<T>(this T control, global::System.Double uniformLength) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => control._set(() => control.Padding = new global::Avalonia.Thickness(uniformLength));

    /// <summary>Gets or sets the padding to place around the .</summary>
    public static T Padding<T>(this T control, global::System.Double horizontal, global::System.Double vertical) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => control._set(() => control.Padding = new global::Avalonia.Thickness(horizontal, vertical));

    /// <summary>Gets or sets the padding to place around the .</summary>
    public static T Padding<T>(this T control, global::System.Double left, global::System.Double top, global::System.Double right, global::System.Double bottom) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => control._set(() => control.Padding = new global::Avalonia.Thickness(left, top, right, bottom));



    /*AvaloniaPropertyBindSetterGenerator*/
    /// <summary>Gets or sets the padding to place around the .</summary>
    public static T Padding<T>(this T control, global::Avalonia.AvaloniaProperty avaloniaProperty, global::Avalonia.Data.BindingMode? bindingMode = null, global::Avalonia.Data.Converters.IValueConverter? converter = null) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => control._set(global::Tests.ValueOverloadsSetterGeneratorTest.PaddingProperty, avaloniaProperty, bindingMode, converter);



    //================= Styles ======================//
    // Padding

    /*ValueStyleSetterGenerator*/
    /// <summary>Gets or sets the padding to place around the .</summary>
    public static global::CSharpUI.Avalonia.Styles.Style<T> Padding<T>(this global::CSharpUI.Avalonia.Styles.Style<T> style, global::Avalonia.Thickness value) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => style._addSetter(global::Tests.ValueOverloadsSetterGeneratorTest.PaddingProperty!, value!);

    /*BindingStyleSetterGenerator*/
    /// <summary>Gets or sets the padding to place around the .</summary>
    public static global::CSharpUI.Avalonia.Styles.Style<T> Padding<T>(this global::CSharpUI.Avalonia.Styles.Style<T> style, global::Avalonia.Data.IBinding binding) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => style._addSetter(global::Tests.ValueOverloadsSetterGeneratorTest.PaddingProperty, binding);

    /*ValueOverloadsStyleSetterGenerator*/
    /// <summary>Gets or sets the padding to place around the .</summary>
    public static global::CSharpUI.Avalonia.Styles.Style<T> Padding<T>(this global::CSharpUI.Avalonia.Styles.Style<T> style, global::System.Double uniformLength) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => style._addSetter(global::Tests.ValueOverloadsSetterGeneratorTest.PaddingProperty, new global::Avalonia.Thickness(uniformLength));

    /// <summary>Gets or sets the padding to place around the .</summary>
    public static global::CSharpUI.Avalonia.Styles.Style<T> Padding<T>(this global::CSharpUI.Avalonia.Styles.Style<T> style, global::System.Double horizontal, global::System.Double vertical) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => style._addSetter(global::Tests.ValueOverloadsSetterGeneratorTest.PaddingProperty, new global::Avalonia.Thickness(horizontal, vertical));

    /// <summary>Gets or sets the padding to place around the .</summary>
    public static global::CSharpUI.Avalonia.Styles.Style<T> Padding<T>(this global::CSharpUI.Avalonia.Styles.Style<T> style, global::System.Double left, global::System.Double top, global::System.Double right, global::System.Double bottom) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => style._addSetter(global::Tests.ValueOverloadsSetterGeneratorTest.PaddingProperty, new global::Avalonia.Thickness(left, top, right, bottom));





}