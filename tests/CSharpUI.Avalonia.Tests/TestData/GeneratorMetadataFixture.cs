using Avalonia;
using Avalonia.Controls;
using CSharpUI.Avalonia;

namespace Tests;

[CSharpUIGeneratorMetadata(Alias = "MetadataFixture")]
public sealed class GeneratorMetadataFixture : Control
{
    public static readonly StyledProperty<string?> DisplayNameProperty =
        AvaloniaProperty.Register<GeneratorMetadataFixture, string?>(nameof(DisplayName));

    [CSharpUIGeneratorMetadata(Alias = "Label")]
    public string? DisplayName
    {
        get => GetValue(DisplayNameProperty);
        set => SetValue(DisplayNameProperty, value);
    }

    [CSharpUIGeneratorMetadata(Ignore = true)]
    public static readonly StyledProperty<string?> InternalNoteProperty =
        AvaloniaProperty.Register<GeneratorMetadataFixture, string?>(nameof(InternalNote));

    public string? InternalNote
    {
        get => GetValue(InternalNoteProperty);
        set => SetValue(InternalNoteProperty, value);
    }
}

public static class GeneratorMetadataFixtureUsage
{
    public static GeneratorMetadataFixture UseAliasedProperty() =>
        CSharpUI.Avalonia.GeneratedControlFactories.MetadataFixture()
            .Label("compiled fixture");
}
