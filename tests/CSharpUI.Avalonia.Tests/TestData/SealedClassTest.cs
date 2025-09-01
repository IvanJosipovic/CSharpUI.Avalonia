using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using CSharpUI.Avalonia;
using System.Windows.Input;

namespace Tests;

public sealed class SealedClassTest : ViewBase
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

    protected override object Build()
    {
        return new TextBox();
    }
}