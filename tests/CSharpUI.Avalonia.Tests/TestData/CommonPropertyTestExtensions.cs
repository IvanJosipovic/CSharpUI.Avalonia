#nullable enable
using System;
using Avalonia.Data;
using Avalonia.Data.Converters;
using System.Runtime.CompilerServices;
using Tests;

namespace CSharpUI.Avalonia;

public static partial class CommonPropertyTestExtensions
{
    // Common Property: MyString
    public static CommonPropertyTest MyString(this CommonPropertyTest control, String value) =>
        control._set(() => control.MyString = value);
}