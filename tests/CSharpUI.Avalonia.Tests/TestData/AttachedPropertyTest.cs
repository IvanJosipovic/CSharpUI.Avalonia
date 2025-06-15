using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using CSharpUI.Avalonia;
using System.Windows.Input;

namespace Tests;

public class AttachedPropertyTest : ViewBase
{
    /// <summary>
    /// My Comment
    /// </summary>
    public static readonly AttachedProperty<ICommand> CommandProperty =
        AvaloniaProperty.RegisterAttached<AttachedPropertyTest, Interactive, ICommand>("Command", default!, false, BindingMode.OneTime);

    /// <summary>
    /// Accessor for Attached property <see cref="CommandProperty"/>.
    /// </summary>
    public static void SetCommand(AvaloniaObject element, ICommand commandValue)
    {
        element.SetValue(CommandProperty, commandValue);
    }

    /// <summary>
    /// Accessor for Attached property <see cref="CommandProperty"/>.
    /// </summary>
    public static ICommand GetCommand(AvaloniaObject element)
    {
        return element.GetValue(CommandProperty);
    }

    protected override object Build()
    {
        return new TextBox();
    }
}