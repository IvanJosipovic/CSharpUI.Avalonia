#nullable enable
using Avalonia.Data;
using Avalonia.Data.Converters;
using CSharpUI.Avalonia.Styles;
using CSharpUI.Avalonia.CommonExtensions;
using Avalonia;
using System;
using Tests;

namespace CSharpUI.Avalonia.Extensions;

public static partial class PropertTest2Extensions
{
    //================= Properties ======================//
    // Padding

    /*ValueSetterGenerator*/
    /// <summary>Gets or sets the padding to place around the .</summary>
    public static T Padding<T>(this T control, Thickness value) where T : PropertTest2
        => control._set(() => control.Padding = value!);

    /*ValueOverloadsSetterGenerator*/

    public static T Padding<T>(this T control, double uniformLength = default) where T : PropertTest2
        => control._set(() => control.Padding = new Thickness(uniformLength));
    public static T Padding<T>(this T control, double horizontal = default, double vertical = default) where T : PropertTest2
        => control._set(() => control.Padding = new Thickness(horizontal, vertical));
    public static T Padding<T>(this T control, double left = default, double top = default, double right = default, double bottom = default) where T : PropertTest2
        => control._set(() => control.Padding = new Thickness(left, top, right, bottom));



    //================= Styles ======================//
    // Padding

    /*ValueStyleSetterGenerator*/
    public static Style<T> Padding<T>(this Style<T> style, Thickness value) where T : PropertTest2
        => style._addSetter(PropertTest2.PaddingProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> Padding<T>(this Style<T> style, IBinding binding) where T : PropertTest2
        => style._addSetter(PropertTest2.PaddingProperty, binding);

    /*ValueOverloadsStyleSetterGenerator*/
    public static Style<T> Padding<T>(this Style<T> style, double uniformLength) where T : PropertTest2
        => style._addSetter(PropertTest2.PaddingProperty, new Thickness(uniformLength));
    public static Style<T> Padding<T>(this Style<T> style, double horizontal, double vertical) where T : PropertTest2
        => style._addSetter(PropertTest2.PaddingProperty, new Thickness(horizontal, vertical));
    public static Style<T> Padding<T>(this Style<T> style, double left, double top, double right, double bottom) where T : PropertTest2
        => style._addSetter(PropertTest2.PaddingProperty, new Thickness(left, top, right, bottom));




}