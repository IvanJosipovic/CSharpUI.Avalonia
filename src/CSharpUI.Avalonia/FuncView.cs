namespace CSharpUI.Avalonia;

/// <summary>
/// View that builds its content using provided function
/// </summary>
/// <typeparam name="TViewModel"></typeparam>
/// <param name="model"></param>
/// <param name="build"></param>
public class FuncView<TViewModel>(TViewModel model, Func<TViewModel, Control> build) : ViewBase<TViewModel>(model)
{
    /// <summary>
    /// Builds content using provided function
    /// </summary>
    /// <param name="vm"></param>
    /// <returns></returns>
    protected override Control Build(TViewModel? vm) => build.Invoke(vm!);
}