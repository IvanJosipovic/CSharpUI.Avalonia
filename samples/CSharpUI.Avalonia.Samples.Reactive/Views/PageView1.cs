using CSharpUI.Avalonia.Samples.Reactive.ViewModels;

namespace CSharpUI.Avalonia.Samples.Reactive.Views;

public class PageView1 : ReactiveViewBase<PageViewModel1>
{
    protected override StyleGroup? BuildStyles() =>
    [
         new Style<TextBlock>()
            .HorizontalAlignment(HorizontalAlignment.Center)
            .VerticalAlignment(VerticalAlignment.Center),

         new Style<TextBox>(x => x.Class("myTextBox"))
            .HorizontalAlignment(HorizontalAlignment.Right)
            .VerticalAlignment(VerticalAlignment.Center)
    ];

    protected override Control Build(PageViewModel1 vm) =>
        new StackPanel()
            .Children([
                new TextBlock()
                    .ReactiveBinding(TextBox.TextProperty, vm, x => x.MyProperty), // One way
                new TextBox()
                    .Class("myTextBox")
                    .ReactiveBinding(TextBox.TextProperty, vm, x => x.MyProperty, x => vm.MyProperty = x ?? ""), // Two way
            ]);
}