#nullable enable
using Avalonia.Data;
using Avalonia.Data.Converters;
using CSharpUI.Avalonia.Styles;
using CSharpUI.Avalonia.CommonExtensions;
using Avalonia;
using System;
using Tests;

namespace CSharpUI.Avalonia.Extensions;

public static partial class SealedClassTestExtensions
{
    //================= Properties ======================//
    // CanSave

    /*ValueSetterGenerator*/
    /// <summary>My Comment</summary>
    public static global::Tests.SealedClassTest CanSave(this global::Tests.SealedClassTest control, bool value)
        => control._set(() => control.CanSave = value!);



}