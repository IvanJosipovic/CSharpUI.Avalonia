using Avalonia.Headless;
using Avalonia.Platform;
using Avalonia.Themes.Simple;
using Avalonia;
using CSharpUI.Avalonia;

[assembly: CollectionBehavior(DisableTestParallelization = true, MaxParallelThreads = 1)]

namespace CSharpUI.Avalonia.Tests;

public abstract class AvaloniaTestBase
{
    static AvaloniaTestBase()
    {
        if (Application.Current is null)
        {
            AppBuilder.Configure<Application>()
                .AfterSetup(builder => builder.Instance?.Styles.Add(new SimpleTheme()))
                .UseHeadless(new AvaloniaHeadlessPlatformOptions
                {
                    UseHeadlessDrawing = true,
                    FrameBufferFormat = PixelFormat.Bgra8888
                })
                .UseViewInitializationStrategy(ViewInitializationStrategy.Lazy)
                .SetupWithoutStarting();
        }
    }
}
