#nullable enable
using CSharpUI.Avalonia.Styles;
using CSharpUI.Avalonia.CommonExtensions;
using Avalonia;
using System;
using Tests;

namespace CSharpUI.Avalonia;

public static partial class DirectPropertyTestExtensions
{
    //================= Properties ======================//
    // CanSave

    /*ValueSetterGenerator*/
    /// <summary></summary>
    public static T CanSave<T>(this T control, Boolean value) where T : DirectPropertyTest
        => control._set(() => control.CanSave = value!);



}