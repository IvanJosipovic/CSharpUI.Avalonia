#nullable enable
using Tests;

namespace CSharpUI.Avalonia;

public static partial class AttachedPropertyTestExtensions
{
    // CanSave
    public static StyledPropertyTest CanSave(this StyledPropertyTest control, Boolean value) =>
        control._set(() => control.CanSave = value);
}