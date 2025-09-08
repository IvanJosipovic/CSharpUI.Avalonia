# CSharpUI.Avalonia

[![Nuget](https://img.shields.io/nuget/vpre/CSharpUI.Avalonia.svg?style=flat-square)](https://www.nuget.org/packages/CSharpUI.Avalonia)
[![Nuget)](https://img.shields.io/nuget/dt/CSharpUI.Avalonia.svg?style=flat-square)](https://www.nuget.org/packages/CSharpUI.Avalonia)
[![codecov](https://codecov.io/github/IvanJosipovic/CSharpUI.Avalonia/graph/badge.svg?token=WXS4sxWpVr)](https://codecov.io/github/IvanJosipovic/CSharpUI.Avalonia)

**CSharpUI.Avalonia** is a fluent, type-safe, XAML-free UI composition library for [Avalonia](https://avaloniaui.net/). It lets you build cross-platform user interfaces entirely in C#, with full IDE support, strong typing.

---

## ✨ Key Features

- **XAML-Free** UI development in pure C#
- **Fluent C# DSL** for layouts, controls, styles, and bindings
- Full **IntelliSense**, **compile-time checking**, and **refactoring support**
- Hot Reload support
- ViewModel binding using **C# Expressions**
- MVVM architecture with **ReactiveUI**
- Source Generator support for external assemblies

---

## 🔧 Installation

```bash
dotnet add package CSharpUI.Avalonia
```

---

## 🖼️ Example View

```csharp
public class PageView : ReactiveViewBase<PageViewModel>
{
    protected override StyleGroup? BuildStyles() =>
    [
         new Style<TextBlock>()
            .HorizontalAlignment(HorizontalAlignment.Center)
            .VerticalAlignment(VerticalAlignment.Center),

         new Style<TextBox>(x => x.Class("myTextBox"))
            .HorizontalAlignment(HorizontalAlignment.Right)
            .VerticalAlignment(VerticalAlignment.Center)
    ];

    protected override Control Build(PageViewModel vm) =>
        new StackPanel()
            .Children([
                new TextBlock()
                    .ReactiveBinding(TextBox.TextProperty, vm, x => x.MyProperty), // One way
                new TextBox()
                    .Class("myTextBox")
                    .ReactiveBinding(TextBox.TextProperty, vm, x => x.MyProperty, x => vm.MyProperty = x ?? ""), // Two way
            ]);
}
```

## 🖼️ Example ViewModel

```csharp
public partial class PageViewModel : ReactiveObject, IRoutableViewModel
{
    public IScreen HostScreen { get; }

    public string? UrlPathSegment => "page";

    public PageViewModel1(IScreen screen)
    {
        HostScreen = screen;
    }

    [Reactive]
    public partial string MyProperty { get; set; } = "Page View";
}
```


## ❤️ Attribution
Includes portions derived from [Avalonia.Markup.Declarative](https://github.com/AvaloniaUI/Avalonia.Markup.Declarative) (MIT License)
