#nullable enable
using System;
using Tests;

namespace CSharpUI.Avalonia;

public static partial class CommonPropertyTestExtensions
{
    //================= Common Properties ======================//
    // MyString

    /*ValueSetterGenerator*/
    /// <summary>My Comment</summary>
    public static CommonPropertyTest MyString(this CommonPropertyTest control, String value)  =>
        control._set(() => control.MyString = value!);



}