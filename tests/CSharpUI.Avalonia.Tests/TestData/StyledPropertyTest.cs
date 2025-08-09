using Avalonia;
using Avalonia.Controls;
using CSharpUI.Avalonia;

namespace Tests;

public class StyledPropertyTest : ViewBase
{
    public static readonly StyledProperty<bool> CanSaveProperty =
        AvaloniaProperty.Register<StyledPropertyTest, bool>(nameof(CanSave));

    /// <summary>
    /// My Comment
    /// </summary>
    public bool CanSave
    {
        get => GetValue(CanSaveProperty);
        set => SetValue(CanSaveProperty, value);
    }

    protected override object Build()
    {
        return new TextBox();
    }
}