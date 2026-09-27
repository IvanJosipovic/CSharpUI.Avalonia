# CSharpUI.Avalonia

[![NuGet](https://img.shields.io/nuget/vpre/CSharpUI.Avalonia.svg?style=flat-square)](https://www.nuget.org/packages/CSharpUI.Avalonia)
[![NuGet downloads](https://img.shields.io/nuget/dt/CSharpUI.Avalonia.svg?style=flat-square)](https://www.nuget.org/packages/CSharpUI.Avalonia)

CSharpUI.Avalonia builds user interfaces in C# with real Avalonia controls. Source generation provides fluent setters, strongly typed expression bindings, event helpers, and style setters. Avalonia continues to own the controls, property system, bindings, styles, and events.

## Requirements

- Avalonia 12.x
- .NET 10 or .NET 11

## Install

```bash
dotnet add package CSharpUI.Avalonia
```

## A view with typed bindings

```csharp
using static CSharpUI.Avalonia.GeneratedControlFactories;

public sealed class GreetingView(GreetingViewModel viewModel) : ViewBase<GreetingViewModel>(viewModel)
{
    protected override object Build(GreetingViewModel vm) =>
        StackPanel()
            .Margin(new Thickness(24))
            .Spacing(12)
            .Children([
                TextBlock()
                    .FontSize(22)
                    .Text(vm, model => model.Greeting),
                TextBox()
                    .PlaceholderText("Type a greeting")
                    .Text(vm, model => model.Greeting, BindingMode.TwoWay)
            ]);
}
```

Generated factory methods create ordinary Avalonia controls and attach their call-site file, line, view type, and control type as source metadata. Import `GeneratedControlFactories` explicitly in files that use the factory syntax. Generated fluent extension methods are produced for Avalonia controls and can also be generated for controls in other assemblies.

`CSharpUIGeneratorMetadataAttribute` can rename generated APIs, suppress generated members or factories, and mark a property as a content slot. For example, `[CSharpUIGeneratorMetadata(Alias = "Caption")]` renames the generated fluent setter while keeping the underlying Avalonia property unchanged.

## Samples

- `CSharpUI.Avalonia.Samples.Basic` builds a view without an MVVM framework.
- `CSharpUI.Avalonia.Samples.Mvvm` shows native `DataContext`, typed bindings, two-way input, and commands.
- `CSharpUI.Avalonia.Samples.CustomControls` shows generated APIs for a local control, including an aliased property, attached property, and event.

See [the architecture guide](docs/architecture.md) for inheritance handling, external assembly generation, source metadata, and generator overrides.

## Build

```bash
dotnet build
dotnet test
```

The library targets `net10.0` and `net11.0`. The source generator targets `netstandard2.0` so it can run in compiler hosts used by supported IDEs.
