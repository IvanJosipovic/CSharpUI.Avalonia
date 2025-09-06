using Avalonia.Controls;

namespace Tests;

public class EventTestGeneric : Control
{
    /// <summary>
    /// Occurs when the
    /// <see cref="P:Avalonia.Controls.AutoCompleteBox.IsDropDownOpen" />
    /// property is changing from true to false.
    /// </summary>
    public event System.EventHandler<System.ComponentModel.CancelEventArgs> DropDownClosing;

    /// <summary>
    /// Occurs when the
    /// <see cref="P:Avalonia.Controls.AutoCompleteBox.IsDropDownOpen" />
    /// property is changing from true to false.
    /// </summary>
    public event System.EventHandler<System.ComponentModel.CancelEventArgs>? DropDownClosing2;

}