#nullable enable
using System;
using Avalonia.Data;
using Avalonia.Data.Converters;
using System.Runtime.CompilerServices;
using Tests;

namespace CSharpUI.Avalonia;

public static partial class AttachedPropertyTestExtensions
{
    // Avalonia Attached Property: CommandProperty
    public static AttachedPropertyTest Command(this AttachedPropertyTest control, System.Windows.Input.ICommand value) =>
        control._set(() => AttachedPropertyTest.SetCommand(control, value));

}