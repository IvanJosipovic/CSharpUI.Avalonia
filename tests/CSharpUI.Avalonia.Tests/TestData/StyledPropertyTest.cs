using Avalonia;
using Avalonia.Controls;

namespace Tests;

public class StyledPropertyTest : Control
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
}