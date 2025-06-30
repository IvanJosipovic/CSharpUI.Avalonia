#nullable enable
using Avalonia.Data;
using Avalonia.Data.Converters;
using CSharpUIAvalonia.Styles;
using CSharpUIAvalonia.CommonExtensions;
using Avalonia;
using System;
using Tests;

namespace CSharpUIAvalonia;

public static partial class DirectPropertyTestExtensions
{
    //================= Properties ======================//
    // CanSave

    /*ValueSetterGenerator*/
    /// <summary>My Comment</summary>
    public static T CanSave<T>(this T control, Boolean value) where T : DirectPropertyTest
        => control._set(() => control.CanSave = value!);



}