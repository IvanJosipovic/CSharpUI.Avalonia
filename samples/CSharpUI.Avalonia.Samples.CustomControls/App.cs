using CSharpUI.Avalonia.Samples.CustomControls.Controls;
using CSharpUI.Avalonia.Samples.CustomControls.Views;

[assembly: GenerateMarkupExtensionsForAvalonia]

var lifetime = new ClassicDesktopStyleApplicationLifetime
{
    Args = args,
    ShutdownMode = ShutdownMode.OnLastWindowClose
};

var appBuilder = AppBuilder.Configure<Application>()
    .UsePlatformDetect()
    .AfterSetup(builder => builder.Instance?.Styles.Add(new FluentTheme()))
#if DEBUG
    .UseHotReload()
#endif
    .SetupWithLifetime(lifetime);

lifetime.MainWindow = new Window
{
    Content = new MainView(),
    Title = "CSharpUI Custom Controls Sample"
};

lifetime.Start(args);
