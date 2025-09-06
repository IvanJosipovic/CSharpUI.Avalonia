using Avalonia.Styling;
using CSharpUI.Avalonia.CommonExtensions;
using CSharpUI.Avalonia.Samples.Reactive.ViewModels;
using CSharpUI.Avalonia.Samples.Reactive.Views;
using CSharpUI.Avalonia.Styles;

namespace CSharpUI.Avalonia.Samples.Reactive.Views;

public class PageView1 : ReactiveViewBase<PageViewModel1>
{
    protected override StyleGroup? BuildStyles() =>
    [
         new Style(x => x.Is<TextBlock>())
             .Setter(HorizontalAlignmentProperty, HorizontalAlignment.Center)
             .Setter(VerticalAlignmentProperty, VerticalAlignment.Center),
         new Style(x => x.Is<TextBox>().Class("myTextBox"))
             .Setter(HorizontalAlignmentProperty, HorizontalAlignment.Center)
             .Setter(VerticalAlignmentProperty, VerticalAlignment.Center),
    ];

    protected override Control Build(PageViewModel1 vm) =>
        new StackPanel()
            .Children([
                new TextBlock()
                    .ReactiveBinding(TextBox.TextProperty, vm, x => x.MyProperty),
                new TextBox()
                    .Class("myTextBox")
                    .ReactiveBinding(TextBox.TextProperty, vm, x => x.MyProperty, x => vm.MyProperty = x ?? ""),
            ]);
}