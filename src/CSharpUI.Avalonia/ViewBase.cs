using CSharpUI.Avalonia.Styles;
using System.Collections.Immutable;
using System.Diagnostics;

namespace CSharpUI.Avalonia;

/// <summary>
/// Generic ViewBase
/// </summary>
/// <typeparam name="TViewModel"></typeparam>
public abstract class ViewBase<TViewModel> : ViewBase
{
    /// <summary>
    /// View Model from the DataContext
    /// </summary>
    public virtual TViewModel? ViewModel
    {
        get => (TViewModel)DataContext!;
        set => DataContext = value;
    }

    /// <summary>
    /// Initializes ViewBase with specified view model
    /// </summary>
    /// <param name="viewModel"></param>
    protected ViewBase(TViewModel viewModel) : base(true)
    {
        DataContext = viewModel;
        OnCreatedCore();
        Initialize();
    }

    /// <inheritdoc/>
    protected abstract object Build(TViewModel? vm);

    /// <inheritdoc/>
    protected override object Build() => Build(ViewModel);
}

/// <summary>
/// Base view class used like UserControl in XAML
/// </summary>

public abstract class ViewBase : Decorator, IReloadable
{
    private INameScope? _nameScope;

    /// <summary>
    /// Current NameScope of this view
    /// </summary>
    protected INameScope Scope => _nameScope ??= new NameScope();

    /// <summary>
    /// Action called when view is initialized
    /// </summary>
    public event Action? ViewInitialized;

    /// <summary>
    /// Called to build the view's content
    /// </summary>
    /// <returns></returns>
    protected abstract object Build();

    /// <summary>
    /// Sets up styles for the view
    /// </summary>
    /// <returns></returns>
    protected virtual StyleGroup? BuildStyles() => null;

    /// <summary>
    /// Initializes ViewBase with deferred loading set to false
    /// </summary>
    protected ViewBase() : this(false)
    {

    }

    /// <summary>
    /// Initializes ViewBase
    /// </summary>
    /// <param name="deferredLoading"></param>
    protected ViewBase(bool deferredLoading)
    {
        if (!deferredLoading)
        {
            OnCreatedCore();
            Initialize();
        }
    }

    /// <summary>
    /// Called after the view is initialized.
    /// </summary>
    protected virtual void OnAfterInitialized() { }

    /// <summary>
    /// Called from constructor, right before initialization and building UI
    /// </summary>
    protected internal void OnCreatedCore() => OnCreated();

    /// <summary>
    /// Called from constructor, right before initialization and building UI
    /// Override this method when you want to run some stuff before creation of children controls
    /// </summary>
    protected virtual void OnCreated()
    {
    }

    /// <summary>
    /// Called to initialize the view, build its content, and apply styles.
    /// </summary>
    /// <exception cref="ViewBuildingException"></exception>
    public void Initialize()
    {
        try
        {
            NameScope.SetNameScope(this, Scope);

            using (var context = new ViewBuildContext(this))
            {
                context.SetState(ViewBuildContextState.StyleBuilding);
                if (BuildStyles() is { } styleGroup)
                {
                    context.SetState(ViewBuildContextState.StyleSelectorUpdating);
                    var viewStyles = StyleBuilder.StylesToRange(styleGroup).ToImmutableList();
                    Styles.AddRange(viewStyles);
                }

                context.SetState(ViewBuildContextState.ViewBuilding);
                var content = Build();
                Child = content as Control;
            }

            ViewInitialized?.Invoke();
            OnAfterInitialized();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
            throw new ViewBuildingException($"Build error in {GetType().Name} : {ex.Message}", ex);
        }
    }

    #region Hot reload stuff
    /// <summary>
    /// Reloads the view by rebuilding its content.
    /// </summary>
    public void Reload()
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            OnBeforeReload();
            Child = null;
            VisualChildren.Clear();
            _nameScope = null;

            OnCreatedCore();
            Initialize();

            InvalidateArrange();
            InvalidateMeasure();
            InvalidateVisual();
        });
    }

    /// <summary>
    /// Called before the view is reloaded.
    /// </summary>
    protected virtual void OnBeforeReload()
    {
    }

    /// <summary>
    /// Called when the view is attached to the visual tree.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (HotReloadManager.IsEnabled)
        {
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
            HotReloadManager.RegisterInstance(this);
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
        }
    }

    /// <summary>
    /// Called when the view is detached from the visual tree.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (HotReloadManager.IsEnabled)
        {
            HotReloadManager.UnregisterInstance(this);
        }
    }
    #endregion
}

internal class ViewBuildContext : IDisposable
{
    private static readonly Stack<ViewBuildContext> ViewsStack = new();
    private static ViewBuildContext? _currentContext;

    internal static ViewBase? CurrentView => _currentContext?._view;
    internal static ViewBuildContextState? CurrentState => _currentContext?._state;

    private readonly ViewBase _view;
    private ViewBuildContextState _state;

    public ViewBuildContext(ViewBase view)
    {
        _view = view;

        if (_currentContext != null)
            ViewsStack.Push(_currentContext);

        _currentContext = this;

        //Debug.WriteLine($"Pushed view {view.GetType().Name}");
    }

    internal void SetState(ViewBuildContextState state)
    {
        _state = state;
    }

    public void Dispose()
    {
        _currentContext = ViewsStack.Count > 0 ? ViewsStack.Pop() : null;

        //if( _currentContext != null )
        //    Debug.WriteLine($"Poped view {_currentContext._view.GetType().Name}");
    }

    //public static string GetViewStackString() => string.Join("/", ViewsStack.ToArray().Reverse().Select(x=>x._view.GetType().Name));
}

internal enum ViewBuildContextState
{
    None,
    StyleBuilding,
    StyleSelectorUpdating,
    ViewBuilding
}

/// <summary>
/// Exception thrown when a view fails to build.
/// </summary>
/// <param name="message"></param>
/// <param name="innerException"></param>
public class ViewBuildingException(string message, Exception innerException) : Exception(message, innerException);