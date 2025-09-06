#nullable enable
using Avalonia.Data;
using Avalonia.Data.Converters;
using CSharpUI.Avalonia.Styles;
using CSharpUI.Avalonia.CommonExtensions;
using Avalonia;
using System;
using Tests;

namespace CSharpUI.Avalonia.Extensions;

public static partial class StyledPropertyTestExtensions
{
    //================= Properties ======================//
    // CanSave

    /*ValueSetterGenerator*/
    /// <summary>My Comment</summary>
    public static T CanSave<T>(this T control, bool value) where T : global::Tests.StyledPropertyTest
        => control._set(() => control.CanSave = value!);



    //================= Styles ======================//
    // CanSave

    /*ValueStyleSetterGenerator*/
    public static Style<T> CanSave<T>(this Style<T> style, bool value) where T : global::Tests.StyledPropertyTest
        => style._addSetter(global::Tests.StyledPropertyTest.CanSaveProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> CanSave<T>(this Style<T> style, IBinding binding) where T : global::Tests.StyledPropertyTest
        => style._addSetter(global::Tests.StyledPropertyTest.CanSaveProperty, binding);



}