#nullable enable
using Avalonia.Data;
using CSharpUI.Avalonia.Styles;
using CSharpUI.Avalonia.CommonExtensions;
using Avalonia;
using System;
using Tests;

namespace CSharpUI.Avalonia.Extensions;

public static partial class AttachedProperty2TestExtensions
{
    //================= Styles ======================//
    // LineSpacing

    /*ValueStyleSetterGenerator*/
    public static Style<T> LineSpacing<T>(this Style<T> style, double value) where T : global::Tests.AttachedProperty2Test
        => style._addSetter(global::Tests.AttachedProperty2Test.LineSpacingProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> LineSpacing<T>(this Style<T> style, IBinding binding) where T : global::Tests.AttachedProperty2Test
        => style._addSetter(global::Tests.AttachedProperty2Test.LineSpacingProperty, binding);



}