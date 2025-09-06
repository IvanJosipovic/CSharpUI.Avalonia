using Avalonia.Controls;

namespace Tests;

public class EventTest : Control
{
    /// <summary>
    /// Occurs when the
    /// <see cref="P:Avalonia.Controls.AutoCompleteBox.IsDropDownOpen" />
    /// property was changed from true to false and the drop-down is open.
    /// </summary>
    public event System.EventHandler DropDownClosed;

    /// <summary>
    /// Occurs when the
    /// <see cref="P:Avalonia.Controls.AutoCompleteBox.IsDropDownOpen" />
    /// property was changed from true to false and the drop-down is open.
    /// </summary>
    public event System.EventHandler? DropDownClosed2;
}