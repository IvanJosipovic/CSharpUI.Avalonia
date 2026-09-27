using CSharpUI.Avalonia;
using Tests;

namespace CSharpUI.Avalonia.Tests;

public sealed class ControlFactoryTests
{
    [Fact]
    public void Generated_factory_returns_real_control_and_records_source_identity()
    {
        var control = GeneratedControlFactories.MetadataFixture("fixture.cs", 27);

        Assert.IsType<GeneratorMetadataFixture>(control);
        var sourceInfo = Assert.IsType<ControlSourceInfo>(SourceInfo.Get(control));
        Assert.Equal("fixture.cs", sourceInfo.FilePath);
        Assert.Equal(27, sourceInfo.LineNumber);
        Assert.Equal(typeof(GeneratorMetadataFixture), sourceInfo.ControlType);
    }

    [Fact]
    public void Metadata_alias_and_ignore_are_applied_to_local_controls()
    {
        var control = GeneratorMetadataFixtureUsage.UseAliasedProperty();
        var extensionType = typeof(GeneratorMetadataFixture).Assembly.GetType(
            "CSharpUI.Avalonia.Tests_GeneratorMetadataFixture_MarkupExtensions",
            throwOnError: true)!;

        Assert.Equal("compiled fixture", control.DisplayName);
        Assert.Contains(extensionType.GetMethods(), static method => method.Name == "Label");
        Assert.DoesNotContain(extensionType.GetMethods(), static method => method.Name == "InternalNote");
    }

    [Fact]
    public void View_factory_uses_generated_local_registration()
    {
        var control = ViewFactory.Create<GeneratorMetadataFixture>();

        Assert.IsType<GeneratorMetadataFixture>(control);
    }
}
