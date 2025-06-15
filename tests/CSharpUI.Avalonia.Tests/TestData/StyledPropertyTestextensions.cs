#nullable enable
using System;
using Avalonia.Data;
using Avalonia.Data.Converters;
using System.Runtime.CompilerServices;
using Tests;

namespace CSharpUI.Avalonia;

public static partial class StyledPropertyTestExtensions
{
    // CanSave
    public static StyledPropertyTest CanSave(this StyledPropertyTest control, Boolean value) =>
        control._set(() => control.CanSave = value);
}