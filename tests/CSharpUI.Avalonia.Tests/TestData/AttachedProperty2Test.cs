using Avalonia;
using Avalonia.Controls;

namespace Tests;

/// <summary>
/// A control that displays a block of text.
/// </summary>
public class AttachedProperty2Test : Control
{
    /// <summary>
    /// Defines the <see cref="LineSpacing"/> property.
    /// </summary>
    public static readonly AttachedProperty<double> LineSpacingProperty =
        AvaloniaProperty.RegisterAttached<TextBlock, Control, double>(
            nameof(LineSpacing),
            0,
            inherits: true);

    /// <summary>
    /// Gets or sets the extra distance of each line to the next line.
    /// </summary>
    public double LineSpacing
    {
        get => GetValue(LineSpacingProperty);
        set => SetValue(LineSpacingProperty, value);
    }
}