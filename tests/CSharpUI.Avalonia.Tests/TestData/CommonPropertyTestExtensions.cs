#nullable enable
using Avalonia.Data;
using Avalonia.Data.Converters;
using CSharpUI.Avalonia.Styles;
using CSharpUI.Avalonia.CommonExtensions;
using System;
using Tests;

namespace CSharpUI.Avalonia.Extensions;

public static partial class CommonPropertyTestExtensions
{
    //================= Common Properties ======================//
    // MyString

    /*ValueSetterGenerator*/
    /// <summary>My Comment</summary>
    public static T MyString<T>(this T control, string value) where T : global::Tests.CommonPropertyTest
        => control._set(() => control.MyString = value!);



}