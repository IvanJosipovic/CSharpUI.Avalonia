using System.Runtime.CompilerServices;
using System.Threading;

namespace CSharpUI.Avalonia;

/// <summary>
/// Identifies the source expression that created a control and the declarative view that owns it.
/// </summary>
/// <param name="FilePath">The source file path.</param>
/// <param name="LineNumber">The one-based source line number.</param>
/// <param name="ControlType">The runtime type of the control.</param>
/// <param name="ViewType">The full name of the owning view type, when the control was created while building a view.</param>
public readonly record struct ControlSourceInfo(string FilePath, int LineNumber, Type ControlType, string? ViewType);

/// <summary>
/// Stores source metadata for controls without changing Avalonia's property or binding behavior.
/// </summary>
public static class SourceInfo
{
    private static readonly ConditionalWeakTable<AvaloniaObject, SourceInfoBox> Entries = new();
    private static readonly AsyncLocal<string?> CurrentViewType = new();

    /// <summary>
    /// Gets source metadata associated with <paramref name="control"/>, if any.
    /// </summary>
    public static ControlSourceInfo? Get(AvaloniaObject control)
    {
        ArgumentNullException.ThrowIfNull(control);
        return Entries.TryGetValue(control, out var box) ? box.Value : null;
    }

    /// <summary>
    /// Associates source metadata with <paramref name="control"/>.
    /// </summary>
    public static void Set(AvaloniaObject control, string? filePath, int lineNumber, string? viewType = null)
    {
        ArgumentNullException.ThrowIfNull(control);
        if (string.IsNullOrWhiteSpace(filePath))
            return;

        Entries.AddOrUpdate(control, new SourceInfoBox(new ControlSourceInfo(
            filePath,
            lineNumber,
            control.GetType(),
            viewType ?? CurrentViewType.Value)));
    }

    /// <summary>
    /// Associates source metadata only if the control does not already have it.
    /// </summary>
    public static void SetIfMissing(AvaloniaObject control, string? filePath, int lineNumber)
    {
        ArgumentNullException.ThrowIfNull(control);
        if (!Entries.TryGetValue(control, out _) && !string.IsNullOrWhiteSpace(filePath))
            Set(control, filePath, lineNumber);
    }

    internal static IDisposable EnterView(Type viewType)
    {
        var previous = CurrentViewType.Value;
        CurrentViewType.Value = viewType.FullName ?? viewType.Name;
        return new ViewScope(previous);
    }

    private sealed record SourceInfoBox(ControlSourceInfo Value);

    private sealed class ViewScope(string? previous) : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
                return;

            CurrentViewType.Value = previous;
            _disposed = true;
        }
    }
}
