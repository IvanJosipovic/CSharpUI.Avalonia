using Avalonia.Styling;
using CSharpUIAvalonia.CommonExtensions;
using CSharpUIAvalonia.Samples.Reactive.ViewModels;
using CSharpUIAvalonia.Samples.Reactive.Views;
using CSharpUIAvalonia.Styles;

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
