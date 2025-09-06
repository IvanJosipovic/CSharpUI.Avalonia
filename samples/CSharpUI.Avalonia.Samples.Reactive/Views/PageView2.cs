using Avalonia.Styling;
using CSharpUI.Avalonia.Samples.Reactive.ViewModels;
using CSharpUI.Avalonia.Styles;
using ReactiveUI;
using System.Reactive.Disposables;

namespace CSharpUI.Avalonia.Samples.Reactive.Views;

public class PageView2 : ReactiveViewBase<PageViewModel2>
{
    private TextBlock tb1 = null!;
    private TextBox tb2 = null!;
    private TextBox tb3 = null!;

    protected override StyleGroup? BuildStyles() =>
    [
         new Style(x => x.Is<TextBlock>())
             .Setter(HorizontalAlignmentProperty, HorizontalAlignment.Center)
             .Setter(VerticalAlignmentProperty, VerticalAlignment.Center),
         new Style(x => x.Is<TextBox>())
             .Setter(HorizontalAlignmentProperty, HorizontalAlignment.Center)
             .Setter(VerticalAlignmentProperty, VerticalAlignment.Center),
    ];

    protected override Control Build(PageViewModel2 vm)
    {
        var controls = new StackPanel()
            .Children([
                new TextBlock()
                    .Ref(out tb1),
                new TextBox()
                    .Ref(out tb2),
                new TextBox()
                    .Ref(out tb3),
            ]);

        this.WhenActivated(disposable =>
        {
            this.OneWayBind(vm, x => x.MyProperty, x => x.tb1.Text)
                .DisposeWith(disposable);
            this.Bind(vm, x => x.MyProperty, x => x.tb2.Text)
                .DisposeWith(disposable);
            this.Bind(vm, x => x.MyProperty, x => x.tb3.Text)
                .DisposeWith(disposable);
        });

        return controls;
    }
}