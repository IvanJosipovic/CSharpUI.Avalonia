using Avalonia;
using CSharpUI.Avalonia.SourceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Tests;

namespace CSharpUI.Avalonia.Tests;

public class ExtensionAvaloniaPropertyExtensionsGeneratorTests
{
    private static string? GetGeneratedOutput(string externalAssemblySourceCode, string className)
    {
        var loadDll = typeof(AvaloniaObject);
        var loadDll1 = typeof(IDeclarativeViewBase);

        var references = AppDomain.CurrentDomain.GetAssemblies()
                          .Where(assembly => !assembly.IsDynamic)
                          .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
                          .Cast<MetadataReference>()
                          .ToList();

        var externalAssemblySyntaxTree = CSharpSyntaxTree.ParseText(externalAssemblySourceCode + "\n public class TestPointer { }");

        var externalAssemblyCompilation = CSharpCompilation.Create("ExternalAssembly",
              [externalAssemblySyntaxTree],
              references,
              new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        references.Add(externalAssemblyCompilation.ToMetadataReference());

        var syntaxTree = CSharpSyntaxTree.ParseText("""
            using CSharpUI.Avalonia;
            using Tests;
            [assembly: GenerateExtensionsForAssembly(typeof(TestPointer))]
            """);

        var compilation = CSharpCompilation.Create("SourceGeneratorTests",
                      [syntaxTree],
                      references,
                      new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));


        // Source Generator to test
        var generator = new ExtensionAvaloniaPropertyExtensionsGenerator();

        CSharpGeneratorDriver.Create(generator)
                             .RunGeneratorsAndUpdateCompilation(compilation,
                                                                out var outputCompilation,
                                                                out var diagnostics);

        // check for errors
        Assert.Empty(diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error));

        var code = outputCompilation.SyntaxTrees.First(x => x.FilePath.EndsWith($"{className}.g.cs")).ToString();

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

        var output = GetGeneratedOutput(inputSource, nameof(DirectPropertyTestExtensions));

        Assert.Equal(output, expectedOutput.Trim());
    }
}
