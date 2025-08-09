using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using CSharpUIAvalonia;
using System.ComponentModel;

namespace Tests;

public class EventTestGeneric : ViewBase
{
    /// <summary>
    /// Occurs when the
    /// <see cref="P:Avalonia.Controls.AutoCompleteBox.IsDropDownOpen" />
    /// property is changing from true to false.
    /// </summary>
    public event EventHandler<CancelEventArgs>? DropDownClosing;

    protected override object Build()
    {
        return new TextBox();
    }
}