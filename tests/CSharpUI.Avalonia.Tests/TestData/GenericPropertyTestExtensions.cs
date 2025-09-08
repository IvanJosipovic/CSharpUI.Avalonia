#nullable enable

namespace CSharpUI.Avalonia.Extensions;

public static partial class GenericPropertyTestExtensions
{
    //================= Properties ======================//
    // Ticks

    /*ValueSetterGenerator*/
    /// <summary></summary>
    public static T Ticks<T>(this T control, global::Avalonia.Collections.AvaloniaList<global::System.Double>? value) where T : global::Tests.GenericPropertyTest
        => control._set(() => control.Ticks = value!);

    /*AvaloniaPropertyBindSetterGenerator*/
    /// <summary></summary>
    public static T Ticks<T>(this T control, global::Avalonia.AvaloniaProperty avaloniaProperty, global::Avalonia.Data.BindingMode? bindingMode = null, global::Avalonia.Data.Converters.IValueConverter? converter = null) where T : global::Tests.GenericPropertyTest
        => control._set(global::Tests.GenericPropertyTest.TicksProperty, avaloniaProperty, bindingMode, converter);



    //================= Styles ======================//
    // Ticks

    /*ValueStyleSetterGenerator*/
    /// <summary></summary>
    public static global::CSharpUI.Avalonia.Styles.Style<T> Ticks<T>(this global::CSharpUI.Avalonia.Styles.Style<T> style, global::Avalonia.Collections.AvaloniaList<global::System.Double>? value) where T : global::Tests.GenericPropertyTest
        => style._addSetter(global::Tests.GenericPropertyTest.TicksProperty!, value!);

    /*BindingStyleSetterGenerator*/
    /// <summary></summary>
    public static global::CSharpUI.Avalonia.Styles.Style<T> Ticks<T>(this global::CSharpUI.Avalonia.Styles.Style<T> style, global::Avalonia.Data.IBinding binding) where T : global::Tests.GenericPropertyTest
        => style._addSetter(global::Tests.GenericPropertyTest.TicksProperty, binding);



}