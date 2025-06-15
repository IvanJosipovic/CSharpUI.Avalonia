#nullable enable
using Tests;

namespace CSharpUI.Avalonia;

public static partial class DirectPropertyTestExtensions
{
    // Avalonia Property: CanSaveProperty
    public static DirectPropertyTest CanSave(this DirectPropertyTest control, bool value) =>
        control._set(() => control.CanSave = value);
}