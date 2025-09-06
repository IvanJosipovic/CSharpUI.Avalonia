using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using CSharpUI.Avalonia;
using System.ComponentModel;

namespace Tests;

public class EventTestGeneric : ViewBase
{
    /// <summary>
    /// Occurs when the
    /// <see cref="P:Avalonia.Controls.AutoCompleteBox.IsDropDownOpen" />
    /// property is changing from true to false.
    /// </summary>
    public event EventHandler<CancelEventArgs> DropDownClosing;

    /// <summary>
    /// Occurs when the
    /// <see cref="P:Avalonia.Controls.AutoCompleteBox.IsDropDownOpen" />
    /// property is changing from true to false.
    /// </summary>
    public event EventHandler<CancelEventArgs>? DropDownClosing2;

    protected override object Build()
    {
        return new TextBox();
    }
}