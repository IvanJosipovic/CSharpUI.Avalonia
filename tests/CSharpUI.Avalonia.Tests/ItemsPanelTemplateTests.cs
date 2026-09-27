using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Styling;
using CSharpUI.Avalonia.Tests;

namespace CSharpUI.Avalonia.Tests;

public sealed class ItemsPanelTemplateTests : AvaloniaTestBase
{
    [Fact]
    public void Items_panel_template_builds_the_configured_panel_through_both_interfaces()
    {
        var panel = new StackPanel();
        var itemsControl = new ItemsControl().ItemsPanel(panel);
        var template = Assert.IsAssignableFrom<ITemplate<Panel?>>(itemsControl.ItemsPanel);

        Assert.Same(panel, template.Build());
        Assert.Same(panel, ((ITemplate)template).Build());
    }
}
