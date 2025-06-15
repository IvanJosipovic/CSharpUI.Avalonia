using CSharpUI.Avalonia;
using Avalonia;
using Avalonia.Controls;

namespace Tests;

public class CommonPropertyTest : ViewBase
{
    public string MyString { get; set; }

    protected override object Build()
    {
        return new TextBox();
    }
}