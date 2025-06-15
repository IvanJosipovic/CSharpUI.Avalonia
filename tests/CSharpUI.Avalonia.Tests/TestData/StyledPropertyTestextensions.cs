#nullable enable
using System;
using Avalonia.Data;
using Avalonia.Data.Converters;
using System.Runtime.CompilerServices;
using Tests;

namespace CSharpUI.Avalonia;

public static partial class StyledPropertyTestExtensions
{
    // Avalonia Property: CanSave
    public static StyledPropertyTest CanSave(this StyledPropertyTest control, bool value) =>
        control._set(() => control.CanSave = value);

}