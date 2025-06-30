using Avalonia;
using CSharpUIAvalonia;
using CSharpUIAvalonia.SourceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Tests;

namespace CSharpUIAvalonia.Tests;

public class GeneratorTests
{
    private static string? GetGeneratedOutput(string sourceCode)
    {
        var loadDll = typeof(AvaloniaObject);
        var loadDll1 = typeof(ViewBase);

        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
        var references = AppDomain.CurrentDomain.GetAssemblies()
                                  .Where(assembly => !assembly.IsDynamic)
                                  .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
                                  .Cast<MetadataReference>();

        var compilation = CSharpCompilation.Create("PropertyGeneratorTests",
                      [syntaxTree],
                      references,
                      new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Release));

        // Source Generator to test
        var generator = new Generator();

        CSharpGeneratorDriver.Create(generator)
                             .RunGeneratorsAndUpdateCompilation(compilation,
                                                                out var outputCompilation,
                                                                out var diagnostics);

        // check for errors
        Assert.Empty(diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error));

        var code = outputCompilation.SyntaxTrees.Skip(1).LastOrDefault()?.ToString();

        return code?.Trim();
    }

    private static (string input, string expected) GetTestSources(string testName, string markupName)
    {
        var inputPath = Path.Combine("TestData", $"{testName}.cs");
        var expectedPath = Path.Combine("TestData", $"{markupName}.cs");
        return (File.ReadAllText(inputPath), File.ReadAllText(expectedPath));
    }

    [Fact]
    public void DirectProperty()
    {
        var (inputSource, expectedOutput) = GetTestSources(nameof(DirectPropertyTest), nameof(DirectPropertyTestExtensions));

        var output = GetGeneratedOutput(inputSource);

        Assert.Equal(output, expectedOutput.Trim());
    }

    [Fact]
    public void StyledProperty()
    {
        var (inputSource, expectedOutput) = GetTestSources(nameof(StyledPropertyTest), nameof(StyledPropertyTestExtensions));

        var output = GetGeneratedOutput(inputSource);

        Assert.Equal(output, expectedOutput.Trim());
    }

    [Fact]
    public void AttachedProperty()
    {
        var (inputSource, expectedOutput) = GetTestSources(nameof(AttachedPropertyTest), nameof(AttachedPropertyTestExtensions));

        var output = GetGeneratedOutput(inputSource);

        Assert.Equal(output, expectedOutput.Trim());
    }

    [Fact]
    public void CommonProperty()
    {
        var (inputSource, expectedOutput) = GetTestSources(nameof(CommonPropertyTest), nameof(CommonPropertyTestExtensions));

        var output = GetGeneratedOutput(inputSource);

        Assert.Equal(output, expectedOutput.Trim());
    }

    [Fact]
    public void GenericBaseTest()
    {
        var (inputSource, expectedOutput) = GetTestSources(nameof(GenericBaseTest), nameof(GenericBaseTestExtensions));

        var output = GetGeneratedOutput(inputSource);

        Assert.Equal(output, expectedOutput.Trim());
    }

    [Fact]
    public void GenericPropertyTest()
    {
        var (inputSource, expectedOutput) = GetTestSources(nameof(GenericPropertyTest), nameof(GenericPropertyTestExtensions));

        var output = GetGeneratedOutput(inputSource);

        Assert.Equal(output, expectedOutput.Trim());
    }
}
