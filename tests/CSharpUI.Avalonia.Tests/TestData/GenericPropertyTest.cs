using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using CSharpUI.Avalonia;

namespace Tests;

public class GenericPropertyTest : ViewBase
{
    //
    // Summary:
    //     Defines the Avalonia.Controls.TickBar.Ticks property.
    public static readonly StyledProperty<AvaloniaList<double>?> TicksProperty;


    //
    // Summary:
    //     The Ticks property contains collection of value of type Double which are the
    //     logical positions use to draw the ticks. The property value is a Avalonia.Collections.AvaloniaList`1.
    public AvaloniaList<double>? Ticks
    {
        get
        {
            return GetValue(TicksProperty);
        }
        set
        {
            SetValue(TicksProperty, value);
        }
    }

    protected override object Build()
    {
        return new TextBox();
    }
}