using Avalonia.Styling;
using CSharpUIAvalonia.CommonExtensions;
using CSharpUIAvalonia.Samples.Reactive.ViewModels;
using CSharpUIAvalonia.Styles;

namespace CSharpUIAvalonia.Samples.Reactive.Views;

public class PageView1 : ReactiveViewBase<PageViewModel1>
{
    protected override StyleGroup? BuildStyles() =>
    [
         new Style(x => x.Is<TextBlock>())
             .Setter(HorizontalAlignmentProperty, HorizontalAlignment.Center)
             .Setter(VerticalAlignmentProperty, VerticalAlignment.Center),
         new Style(x => x.Is<TextBox>())
             .Setter(HorizontalAlignmentProperty, HorizontalAlignment.Center)
             .Setter(VerticalAlignmentProperty, VerticalAlignment.Center),
         //new Style(x => x.Is<TextBox>().Class("bob"))
         //    .Setter(Control.HorizontalAlignmentProperty, HorizontalAlignment.Center)
         //    .Setter(Control.VerticalAlignmentProperty, VerticalAlignment.Center),
    ];

    protected override Control Build(PageViewModel1 vm) =>
        new StackPanel()
            .Children([
                new TextBlock()
                    .ReactiveBinding(TextBox.TextProperty, vm, x => x.MyProperty),
                //new TextBlock()
                //    .ReactiveBinding(TextBox.TextProperty, vm, x => x.MyProperty),
                new TextBox()
                    .ReactiveBinding(TextBox.TextProperty, vm, x => x.MyProperty, x => vm.MyProperty = x ?? "1"),
                new TextBox()
                    .ReactiveBinding(TextBox.TextProperty, vm, x => x.MyProperty, x => vm.MyProperty = x ?? ""),
            ]);
}
