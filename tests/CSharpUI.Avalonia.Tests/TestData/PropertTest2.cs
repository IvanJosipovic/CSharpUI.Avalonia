using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using CSharpUI.Avalonia;
using System.Windows.Input;

namespace Tests;

public class PropertTest2 : ViewBase
{
    /// <summary>
    /// Defines the <see cref="Padding"/> property.
    /// </summary>
    public static readonly StyledProperty<Thickness> PaddingProperty =
        Decorator.PaddingProperty.AddOwner<PropertTest2>();

    /// <summary>
    /// Gets or sets the padding to place around the <see cref="Text"/>.
    /// </summary>
    public Thickness Padding
    {
        get => GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    protected override object Build()
    {
        return new TextBox();
    }
}