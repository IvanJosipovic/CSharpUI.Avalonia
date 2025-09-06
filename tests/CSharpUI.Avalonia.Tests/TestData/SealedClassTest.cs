using Avalonia;
using Avalonia.Controls;

namespace Tests;

public sealed class SealedClassTest : Control
{
    private bool _canSave = default;

    public static readonly DirectProperty<SealedClassTest, bool> CanSaveProperty =
        AvaloniaProperty.RegisterDirect<SealedClassTest, bool>
        (
            nameof(CanSave),
            o => o.CanSave
        );

    /// <summary>
    /// My Comment
    /// </summary>
    public bool CanSave
    {
        get => _canSave;
        set => SetAndRaise(CanSaveProperty, ref _canSave, value);
    }
}