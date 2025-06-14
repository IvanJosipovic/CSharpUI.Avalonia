using ReactiveUI;

namespace CSharpUI.Avalonia.Samples.Reactive.Views;

public abstract class ReactiveViewBase<TViewModel> : ViewBase, IViewFor<TViewModel> where TViewModel : class
{
    private object? _cachedDataContext;

    public TViewModel? ViewModel
    {
        get { return GetValue(DataContextProperty) as TViewModel; }
        set { SetValue(DataContextProperty, value); }
    }

    object? IViewFor.ViewModel
    {
        get { return GetValue(DataContextProperty); }
        set { SetValue(DataContextProperty, value); }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext != null)
        {
            OnCreatedCore();
            Initialize();
        }
    }

    protected ReactiveViewBase() : base(deferredLoading: true)
    {
    }

    protected abstract Control Build(TViewModel vm);

    protected override Control Build() => Build((TViewModel)(_cachedDataContext ?? DataContext!));

    protected override void OnBeforeReload()
    {
        _cachedDataContext = DataContext;
    }
}
