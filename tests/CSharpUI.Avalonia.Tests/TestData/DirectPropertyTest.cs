using Avalonia;
using Avalonia.Controls;

namespace Tests;

public class DirectPropertyTest : Control
{
    private bool _canSave = default;

    public static readonly DirectProperty<DirectPropertyTest, bool> CanSaveProperty =
        AvaloniaProperty.RegisterDirect<DirectPropertyTest, bool>
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