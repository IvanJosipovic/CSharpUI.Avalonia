using CSharpUIAvalonia.Samples.Reactive.ViewModels;
using CSharpUIAvalonia.Samples.Reactive.Views;
using ReactiveUI;

namespace CSharpUIAvalonia.Samples.Reactive;

public class AppViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T? viewModel, string? contract = null) => viewModel switch
    {
        PageViewModel1 context => new PageView1 { DataContext = context },
        PageViewModel2 context => new PageView2 { DataContext = context },
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}