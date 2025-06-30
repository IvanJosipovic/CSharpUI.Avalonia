using CSharpUIAvalonia;
using ReactiveUI;

namespace Tests;

public abstract class GenericBaseTest<TViewModel> : ViewBase, IViewFor<TViewModel> where TViewModel : class
{
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
}
