# CSharpUI.Avalonia

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

## Example View

```csharp
public class PageView : ReactiveViewBase<PageViewModel>
{
    protected override StyleGroup? BuildStyles() =>
    [
         new Style(x => x.Is<TextBlock>())
             .Setter(HorizontalAlignmentProperty, HorizontalAlignment.Center)
             .Setter(VerticalAlignmentProperty, VerticalAlignment.Center),
         new Style(x => x.Is<TextBox>().Class("myTextBox"))
             .Setter(HorizontalAlignmentProperty, HorizontalAlignment.Center)
             .Setter(VerticalAlignmentProperty, VerticalAlignment.Center),
    ];

    protected override Control Build(PageViewModel vm) =>
        new StackPanel()
            .Children([
                new TextBlock()
                    .ReactiveBinding(TextBox.TextProperty, vm, x => x.MyProperty),
                new TextBox()
                    .Class("myTextBox")
                    .ReactiveBinding(TextBox.TextProperty, vm, x => x.MyProperty, x => vm.MyProperty = x ?? ""),
            ]);
}
```


## ❤️ Attribution
Includes portions derived from [Avalonia.Markup.Declarative](https://github.com/AvaloniaUI/Avalonia.Markup.Declarative) (MIT License)
