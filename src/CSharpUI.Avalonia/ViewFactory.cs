using Avalonia.Controls;
using System.Diagnostics.CodeAnalysis;

namespace CSharpUI.Avalonia;

/// <summary>
/// Factory for creating control instances with optional dependency injection.
/// </summary>
public static class ViewFactory
{
    /// <summary>
    /// Creates a control instance using the configured control factory
    /// or falls back to parameterless constructor.
    /// </summary>
    public static TControl Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TControl>() where TControl : Control
    {
        if (AppBuilderExtensions.ControlFactory is { } factory)
            return (TControl)factory(typeof(TControl));

        if (AppBuilderExtensions.ServiceProvider?.GetService(typeof(TControl)) is TControl serviceControl)
            return serviceControl;

        if (ViewFactoryRegistry.TryCreate<TControl>(out var registeredControl))
            return registeredControl;

        return System.Activator.CreateInstance<TControl>();
    }
}
