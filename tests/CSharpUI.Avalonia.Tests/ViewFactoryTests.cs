using Avalonia.Controls;
using CSharpUI.Avalonia.Tests;

namespace CSharpUI.Avalonia.Tests.Controls;

public sealed class ViewFactoryTests : AvaloniaTestBase
{
    [Fact]
    public void Create_uses_generated_factory_for_internal_control()
    {
        var control = ViewFactory.Create<InternalFactoryControl>();

        Assert.IsType<InternalFactoryControl>(control);
    }
}

internal sealed class InternalFactoryControl : Button
{
    internal InternalFactoryControl()
    {
    }
}
