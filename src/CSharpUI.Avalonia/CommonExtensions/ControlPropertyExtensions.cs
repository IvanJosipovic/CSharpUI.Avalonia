using Avalonia.Controls;
using Avalonia.Media;

namespace CSharpUI.Avalonia.CommonExtensions;

/// <summary>
/// Additional fluent helpers that are independent of generated Avalonia property setters.
/// </summary>
public static class ControlPropertyExtensions
{
    /// <summary>
    /// Converts a color to a solid color brush.
    /// </summary>
    public static SolidColorBrush ToBrush(this Color color) => new(color);

    /// <summary>
    /// Adds a CSS-style class name to a control and returns the control.
    /// </summary>
    public static TControl Class<TControl>(this TControl control, string className)
        where TControl : Control
    {
        control.Classes.Add(className);
        return control;
    }
}
