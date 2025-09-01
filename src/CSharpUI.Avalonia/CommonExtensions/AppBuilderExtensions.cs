namespace CSharpUI.Avalonia.CommonExtensions;

/// <summary>
/// Provides extension methods for configuring an <see cref="AppBuilder"/>.
/// </summary>
public static class AppBuilderExtensions
{
    private static IServiceProvider? _serviceProvider;

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/> instance.
    /// </summary>
    internal static IServiceProvider? ServiceProvider => _serviceProvider;

    /// <summary>
    /// Configures the <see cref="AppBuilder"/> to use the specified <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="appBuilder">The <see cref="AppBuilder"/> to configure.</param>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to use.</param>
    /// <returns>The configured <see cref="AppBuilder"/>.</returns>
    public static AppBuilder UseServiceProvider(this AppBuilder appBuilder, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        return appBuilder;
    }

    /// <summary>
    /// Enables or disables hot reload functionality for the application.
    /// </summary>
    /// <param name="appBuilder">The <see cref="AppBuilder"/> to configure.</param>
    /// <param name="enable">A value indicating whether to enable hot reload. Defaults to <c>true</c>.</param>
    /// <returns>The configured <see cref="AppBuilder"/>.</returns>
    public static AppBuilder UseHotReload(this AppBuilder appBuilder, bool enable = true)
    {
        if (enable)
        {
            HotReloadManager.Activate();
        }
        else
        {
            HotReloadManager.Deactivate();
        }
        return appBuilder;
    }
}