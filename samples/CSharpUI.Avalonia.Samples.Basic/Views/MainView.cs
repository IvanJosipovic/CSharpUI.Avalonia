using static CSharpUI.Avalonia.GeneratedControlFactories;

namespace CSharpUI.Avalonia.Samples.Basic.Views;

public sealed class MainView : ViewBase
{
    protected override object Build() =>
        StackPanel()
            .Margin(new Thickness(24))
            .Spacing(12)
            .Children([
                TextBlock()
                    .FontSize(22)
                    .Text("Hello from CSharpUI"),
                Button()
                    .Content("A real Avalonia button")
            ]);
}
