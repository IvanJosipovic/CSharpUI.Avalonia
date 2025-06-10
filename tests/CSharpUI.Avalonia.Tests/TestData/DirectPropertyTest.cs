using CSharpUI.Avalonia;
using Avalonia;
using Avalonia.Controls;

namespace Tests;

public class DirectPropertyTest : ViewBase
{
    private bool _canSave = default;

    public static readonly DirectProperty<DirectPropertyTest, bool> CanSaveProperty =
        AvaloniaProperty.RegisterDirect<DirectPropertyTest, bool>
        (
            nameof(CanSave),
            o => o.CanSave
        );

    public bool CanSave
    {
        get => _canSave;
        set => SetAndRaise(CanSaveProperty, ref _canSave, value);
    }

    protected override object Build()
    {
        return new TextBox();
    }
}