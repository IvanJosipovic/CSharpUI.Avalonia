namespace CSharpUI.Avalonia;

public class FuncView<TViewModel>(TViewModel model, Func<TViewModel, Control> build) : ViewBase<TViewModel>(model)
{
    protected override Control Build(TViewModel? vm) => build.Invoke(vm!);
}