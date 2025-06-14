#nullable enable
using System;
using Avalonia.Data;
using Avalonia.Data.Converters;
using System.Runtime.CompilerServices;
using Tests;

namespace CSharpUI.Avalonia;

public static partial class DirectPropertyTestExtensions
{
    // common properties
    public static DirectPropertyTest CanSave(this DirectPropertyTest control, Boolean value) =>
         control._set(() => control.CanSave = value);
}