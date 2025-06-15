using Avalonia;
using CSharpUI.Avalonia.SourceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Tests;

namespace CSharpUI.Avalonia.Tests;

public class PropertyGeneratorTests
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
                      new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        // Source Generator to test
        var generator = new PropertyGenerator();

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
    public void CommonPropertyTest()
    {
        var (inputSource, expectedOutput) = GetTestSources(nameof(CommonPropertyTest), nameof(CommonPropertyTestExtensions));

        var output = GetGeneratedOutput(inputSource);

        Assert.Equal(output, expectedOutput.Trim());
    }
}
