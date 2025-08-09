using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using CSharpUIAvalonia;
using System.ComponentModel;

namespace Tests;

public class EventTest : ViewBase
{
    /// <summary>
    /// Occurs when the
    /// <see cref="P:Avalonia.Controls.AutoCompleteBox.IsDropDownOpen" />
    /// property was changed from true to false and the drop-down is open.
    /// </summary>
    public event EventHandler? DropDownClosed;

    protected override object Build()
    {
        return new TextBox();
    }
}