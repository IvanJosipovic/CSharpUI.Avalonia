using Avalonia.ReactiveUI;
using CSharpUI.Avalonia.CommonExtensions;
using CSharpUI.Avalonia.Samples.Reactive.ViewModels;
using CSharpUI.Avalonia.Samples.Reactive.Views;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
var lifetime = new ClassicDesktopStyleApplicationLifetime
{
    Args = args,
    ShutdownMode = ShutdownMode.OnLastWindowClose
};

var appBuilder = AppBuilder.Configure<Application>()
    .UsePlatformDetect()
    .AfterSetup(b => b.Instance?.Styles.Add(new FluentTheme()))
    .UseServiceProvider(services.BuildServiceProvider())
    .UseReactiveUI()
#if DEBUG
    .UseHotReload()
#endif
    .SetupWithLifetime(lifetime);

lifetime.MainWindow = new Window()
                            .Content(new MainView(new MainViewModel()))
                            .Title("Reactive Sample App");

#if DEBUG
lifetime.MainWindow.AttachDevTools();
#endif

lifetime.Start(args);