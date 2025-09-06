using Avalonia;
using CSharpUI.Avalonia.Extensions;
using CSharpUI.Avalonia.SourceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Tests;

namespace CSharpUI.Avalonia.Tests;

public class GeneratorTests
{
    [Theory]
    [InlineData(typeof(AttachedProperty2Test))]
    [InlineData(typeof(AttachedProperty3Test))]
    [InlineData(typeof(AttachedPropertyTest))]
    [InlineData(typeof(CommonPropertyTest))]
    [InlineData(typeof(DirectPropertyTest))]
    [InlineData(typeof(EventTest))]
    [InlineData(typeof(EventTestGeneric))]
    [InlineData(typeof(GenericBaseTest<>))]
    [InlineData(typeof(GenericPropertyTest))]
    [InlineData(typeof(SealedClassTest))]
    [InlineData(typeof(StyledPropertyTest))]
    [InlineData(typeof(ValueOverloadsSetterGeneratorTest))]

    public void Text(Type type)
    {
        var name = type.Name;

        if (type.IsGenericType)
        {
            name = type.GetGenericTypeDefinition().Name;
            // Remove the arity suffix (e.g., "`1")
            var tickIndex = name.IndexOf('`');
            name = tickIndex > 0 ? name[..tickIndex] : name;
        }

        var (inputSource, expectedOutput) = GetTestSources(name, name + "Extensions");

        var output = GetGeneratedOutput(inputSource);

        Assert.Equal(expectedOutput.Trim(), output);
    }

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
}