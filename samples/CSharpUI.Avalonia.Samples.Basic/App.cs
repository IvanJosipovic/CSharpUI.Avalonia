using CSharpUI.Avalonia.Samples.Basic.Views;

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
    Title = "CSharpUI Basic Sample"
};

lifetime.Start(args);
