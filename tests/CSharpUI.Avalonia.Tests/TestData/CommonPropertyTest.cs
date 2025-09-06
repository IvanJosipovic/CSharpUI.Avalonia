using Avalonia;
using Avalonia.Controls;
using CSharpUI.Avalonia;

namespace Tests;

public class CommonPropertyTest : ViewBase
{
    /// <summary>
    /// My Comment
    /// </summary>
    public string MyString { get; set; } = "";

    protected override object Build()
    {
        return new TextBox();
    }
}