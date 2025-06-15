using Avalonia;
using CSharpUI.Avalonia.SourceGenerator.External;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Tests;

namespace CSharpUI.Avalonia.Tests;

public class ExternalPropertyGeneratorTests
{
    private static string? GetGeneratedOutput(string externalAssemblySourceCode)
    {
        var loadDll = typeof(AvaloniaObject);
        var loadDll1 = typeof(ViewBase);

        var references = AppDomain.CurrentDomain.GetAssemblies()
                          .Where(assembly => !assembly.IsDynamic)
                          .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
                          .Cast<MetadataReference>()
                          .ToList();

        var externalAssemblySyntaxTree = CSharpSyntaxTree.ParseText(externalAssemblySourceCode + "\n\r" + "public class TestPointer { }");

        var externalAssemblyCompilation = CSharpCompilation.Create("ExternalAssembly",
              [externalAssemblySyntaxTree],
              references,
              new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var stream = new MemoryStream();
        var results = externalAssemblyCompilation.Emit(stream);

        if (!results.Success)
        {
            throw new Exception(results.Diagnostics.First().GetMessage());
        }

        stream.Position = 0;
        references.Add(MetadataReference.CreateFromStream(stream));

        var syntaxTree = CSharpSyntaxTree.ParseText("""
            using CSharpUI.Avalonia;
            using Tests;
            [assembly: GenerateExtensionsForAssembly(typeof(TestPointer))]
            [assembly: GenerateExtensionsForAssembly(typeof(TestPointer))]
            """);

        var compilation = CSharpCompilation.Create("SourceGeneratorTests",
                      [syntaxTree],
                      references,
                      new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));


        // Source Generator to test
        var generator = new ExternalPropertyGenerator();

        CSharpGeneratorDriver.Create(generator)
                             .RunGeneratorsAndUpdateCompilation(compilation,
                                                                out var outputCompilation,
                                                                out var diagnostics);

        // check for errors
        Assert.Empty(diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error));

        var code = outputCompilation.SyntaxTrees.Skip(1).Last(x => !x.FilePath.EndsWith("TestPointerExtensions.g.cs")).ToString();

        // remove // Auto-generated code <date/time>
        if (code != null)
        {
            var lines = code.Split([Environment.NewLine], StringSplitOptions.None);
            code = string.Join(Environment.NewLine, lines.Where(line => !line.TrimStart().StartsWith("// Auto-generated code")));
        }

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
}
