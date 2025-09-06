#nullable enable
using Avalonia.Data;
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
    public static T Ticks<T>(this T control, global::Avalonia.Collections.AvaloniaList<double>? value) where T : global::Tests.GenericPropertyTest
        => control._set(() => control.Ticks = value!);



    //================= Styles ======================//
    // Ticks

    /*ValueStyleSetterGenerator*/
    public static Style<T> Ticks<T>(this Style<T> style, global::Avalonia.Collections.AvaloniaList<double>? value) where T : global::Tests.GenericPropertyTest
        => style._addSetter(global::Tests.GenericPropertyTest.TicksProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> Ticks<T>(this Style<T> style, IBinding binding) where T : global::Tests.GenericPropertyTest
        => style._addSetter(global::Tests.GenericPropertyTest.TicksProperty, binding);



}