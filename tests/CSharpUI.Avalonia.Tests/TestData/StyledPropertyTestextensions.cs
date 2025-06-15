#nullable enable
using Avalonia.Data;
using Avalonia.Data.Converters;
using CSharpUI.Avalonia.Styles;
using CSharpUI.Avalonia.CommonExtensions;
using Avalonia;
using System;
using Tests;

namespace CSharpUI.Avalonia;

public static partial class StyledPropertyTestExtensions
{
    //================= Properties ======================//
    // CanSave

    /*ValueSetterGenerator*/
    /// <summary>My Comment</summary>
    public static T CanSave<T>(this T control, Boolean value) where T : StyledPropertyTest
        => control._set(() => control.CanSave = value!);




    //================= Styles ======================//
    // CanSave

    /*ValueStyleSetterGenerator*/
    public static Style<T> CanSave<T>(this Style<T> style, Boolean value) where T : StyledPropertyTest
        => style._addSetter(StyledPropertyTest.CanSaveProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> CanSave<T>(this Style<T> style, IBinding binding) where T : StyledPropertyTest
        => style._addSetter(StyledPropertyTest.CanSaveProperty, binding);



}