#nullable enable
using Avalonia.Data;
using Avalonia.Data.Converters;
using CSharpUIAvalonia.Styles;
using CSharpUIAvalonia.CommonExtensions;
using System;
using Tests;

namespace CSharpUIAvalonia;

public static partial class CommonPropertyTestExtensions
{
    //================= Common Properties ======================//
    // MyString

    /*ValueSetterGenerator*/
    /// <summary>My Comment</summary>
    public static CommonPropertyTest MyString(this CommonPropertyTest control, String value)
        => control._set(() => control.MyString = value!);



}