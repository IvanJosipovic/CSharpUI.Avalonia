#nullable enable
using Avalonia.Data;
using CSharpUI.Avalonia.Styles;
using CSharpUI.Avalonia.CommonExtensions;
using Avalonia;
using System;
using Tests;

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

    public static T Padding<T>(this T control, double uniformLength = default) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => control._set(() => control.Padding = new global::Avalonia.Thickness(uniformLength));
    public static T Padding<T>(this T control, double horizontal = default, double vertical = default) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => control._set(() => control.Padding = new global::Avalonia.Thickness(horizontal, vertical));
    public static T Padding<T>(this T control, double left = default, double top = default, double right = default, double bottom = default) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => control._set(() => control.Padding = new global::Avalonia.Thickness(left, top, right, bottom));



    //================= Styles ======================//
    // Padding

    /*ValueStyleSetterGenerator*/
    public static Style<T> Padding<T>(this Style<T> style, global::Avalonia.Thickness value) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => style._addSetter(global::Tests.ValueOverloadsSetterGeneratorTest.PaddingProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> Padding<T>(this Style<T> style, IBinding binding) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => style._addSetter(global::Tests.ValueOverloadsSetterGeneratorTest.PaddingProperty, binding);

    /*ValueOverloadsStyleSetterGenerator*/
    public static Style<T> Padding<T>(this Style<T> style, double uniformLength) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => style._addSetter(global::Tests.ValueOverloadsSetterGeneratorTest.PaddingProperty, new global::Avalonia.Thickness(uniformLength));
    public static Style<T> Padding<T>(this Style<T> style, double horizontal, double vertical) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => style._addSetter(global::Tests.ValueOverloadsSetterGeneratorTest.PaddingProperty, new global::Avalonia.Thickness(horizontal, vertical));
    public static Style<T> Padding<T>(this Style<T> style, double left, double top, double right, double bottom) where T : global::Tests.ValueOverloadsSetterGeneratorTest
        => style._addSetter(global::Tests.ValueOverloadsSetterGeneratorTest.PaddingProperty, new global::Avalonia.Thickness(left, top, right, bottom));




}