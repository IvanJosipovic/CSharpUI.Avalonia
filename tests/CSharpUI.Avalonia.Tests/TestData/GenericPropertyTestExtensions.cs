#nullable enable
using Avalonia.Data;
using Avalonia.Data.Converters;
using CSharpUI.Avalonia.Styles;
using CSharpUI.Avalonia.CommonExtensions;
using Avalonia;
using Avalonia.Collections;
using System;
using Tests;

namespace CSharpUI.Avalonia.Extensions;

public static partial class GenericPropertyTestExtensions
{
    //================= Properties ======================//
    // Ticks

    /*ValueSetterGenerator*/
    /// <summary></summary>
    public static T Ticks<T>(this T control, AvaloniaList<Double>? value) where T : GenericPropertyTest
        => control._set(() => control.Ticks = value!);



    //================= Styles ======================//
    // Ticks

    /*ValueStyleSetterGenerator*/
    public static Style<T> Ticks<T>(this Style<T> style, AvaloniaList<Double>? value) where T : GenericPropertyTest
        => style._addSetter(GenericPropertyTest.TicksProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> Ticks<T>(this Style<T> style, IBinding binding) where T : GenericPropertyTest
        => style._addSetter(GenericPropertyTest.TicksProperty, binding);



}