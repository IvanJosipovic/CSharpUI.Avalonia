#nullable enable

namespace CSharpUI.Avalonia.Extensions;

public static partial class GenericPropertyTestExtensions
{
    //================= Properties ======================//
    // Ticks

    /*ValueSetterGenerator*/
    /// <summary></summary>
    public static T Ticks<T>(this T control, global::Avalonia.Collections.AvaloniaList<double>? value) where T : global::Tests.GenericPropertyTest
        => control._set(() => control.Ticks = value!);



    //================= Styles ======================//
    // Ticks

    /*ValueStyleSetterGenerator*/
    public static global::CSharpUI.Avalonia.Styles.Style<T> Ticks<T>(this global::CSharpUI.Avalonia.Styles.Style<T> style, global::Avalonia.Collections.AvaloniaList<double>? value) where T : global::Tests.GenericPropertyTest
        => style._addSetter(global::Tests.GenericPropertyTest.TicksProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static global::CSharpUI.Avalonia.Styles.Style<T> Ticks<T>(this global::CSharpUI.Avalonia.Styles.Style<T> style, global::Avalonia.Data.IBinding binding) where T : global::Tests.GenericPropertyTest
        => style._addSetter(global::Tests.GenericPropertyTest.TicksProperty, binding);



}