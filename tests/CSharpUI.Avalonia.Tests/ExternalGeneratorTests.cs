using CSharpUI.Avalonia.SourceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Tests;

namespace CSharpUI.Avalonia.Tests;

public class ExternalPropertyGeneratorTests
{

    [Theory]
    [InlineData(typeof(AttachedProperty2Test))]
    [InlineData(typeof(AttachedProperty3Test))]
    [InlineData(typeof(AttachedPropertyTest))]
    [InlineData(typeof(CommentTest))]
    [InlineData(typeof(CommonPropertyTest))]
    [InlineData(typeof(DirectPropertyTest))]
    [InlineData(typeof(EventTest))]
    [InlineData(typeof(EventTestGeneric))]
    [InlineData(typeof(GenericBaseTest<>))]
    [InlineData(typeof(GenericPropertyTest))]
    [InlineData(typeof(SealedClassTest))]
    [InlineData(typeof(StyledPropertyTest))]
    [InlineData(typeof(ValueOverloadsSetterGeneratorTest))]
    public void Test(Type type)
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

    private static string? GetGeneratedOutput(string externalAssemblySourceCode)
    {
        var references = AppDomain.CurrentDomain.GetAssemblies()
                  .Where(assembly => !assembly.IsDynamic)
                  .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
                  .Cast<MetadataReference>()
                  .ToList();

        var externalAssemblySyntaxTree = CSharpSyntaxTree.ParseText(externalAssemblySourceCode + "\n\r" + "public class TestPointer { }");

        var externalAssemblyCompilation = CSharpCompilation.Create("ExternalAssembly",
              [externalAssemblySyntaxTree],
              references,
              new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithConcurrentBuild(true)
                .WithDeterministic(true)
                .WithNullableContextOptions(NullableContextOptions.Enable)
                .WithOptimizationLevel(OptimizationLevel.Release)
                .WithOverflowChecks(false)
                .WithPlatform(Platform.AnyCpu)
              );

        var dll = new MemoryStream();
        var comments = new MemoryStream();
        var results = externalAssemblyCompilation.Emit(dll, null, comments);

        if (!results.Success)
        {
            var failures = results.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

            throw new Exception(failures.Select(x => $"Error creating Assembly: {x.Id}: {x.GetMessage()}").Aggregate((x,y) => x + "\n\r" + y));
        }

        dll.Position = 0;
        comments.Position = 0;
        references.Add(MetadataReference.CreateFromStream(dll, documentation: XmlDocumentationProvider.CreateFromBytes(comments.ToArray())));

        var syntaxTree = CSharpSyntaxTree.ParseText("""
            using CSharpUI.Avalonia;
            using Tests;
            [assembly: GenerateExtensionsForAssembly(typeof(TestPointer))]
            """);

        var compilation = CSharpCompilation.Create("ExternalPropertyGeneratorTests",
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

        var code = outputCompilation.SyntaxTrees.Skip(1).Last(x => !x.FilePath.EndsWith("TestPointerExtensions.g.cs")).ToString();

        return code?.Trim();
    }

    private static (string input, string expected) GetTestSources(string testName, string markupName)
    {
        var inputPath = Path.Combine("TestData", $"{testName}.cs");
        var expectedPath = Path.Combine("TestData", $"{markupName}.cs");
        return (File.ReadAllText(inputPath), File.ReadAllText(expectedPath));
    }
}