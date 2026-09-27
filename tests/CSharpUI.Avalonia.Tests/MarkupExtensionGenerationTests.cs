using System.Collections.Immutable;
using Avalonia;
using Avalonia.Controls;
using CSharpUI.Avalonia;
using CSharpUI.Avalonia.SourceGenerator.ExternalGenerators;
using CSharpUI.Avalonia.SourceGenerator.InternalGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Runtime.InteropServices;
using System.Reflection;

namespace CSharpUI.Avalonia.Tests.SourceGeneratorTests;

public class MarkupExtensionGenerationTests
{
    private const string FixtureNamespace = "MarkupExtensionGeneratorFixture";
    private static readonly ImmutableArray<MetadataReference> PlatformReferences = CreatePlatformReferences();

    // Expected sources are from Avalonia.Markup.Declarative commit
    // 619df233a648cd7cddb40de9d0e437ef9b452832, normalized for CSharpUI's namespace and generator identity.
    [Fact]
    public void Compiled_fixture_extensions_match_the_pinned_amd_output()
    {
        var assembly = typeof(MarkupExtensionGenerationTests).Assembly;
        var generatedSourcesPath = assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(static metadata => metadata.Key == "CSharpUI.GeneratedSourcesPath")
            .Value;
        Assert.False(string.IsNullOrWhiteSpace(generatedSourcesPath));

        var expectedSourcesPath = Path.Combine(AppContext.BaseDirectory, "TestData");
        var expectedFiles = Directory.GetFiles(expectedSourcesPath, "*Extensions.cs");
        Assert.NotEmpty(expectedFiles);

        foreach (var expectedFile in expectedFiles)
        {
            var fixtureName = Path.GetFileNameWithoutExtension(expectedFile);
            Assert.EndsWith("Extensions", fixtureName, StringComparison.Ordinal);
            var controlName = fixtureName[..^"Extensions".Length];
            var generatedFileName = $"Local_Tests.{controlName}.g.cs";
            var generatedFile = Assert.Single(Directory.GetFiles(
                generatedSourcesPath!, generatedFileName, SearchOption.AllDirectories));

            Assert.Equal(
                NormalizeGeneratedSource(File.ReadAllText(expectedFile)),
                NormalizeGeneratedSource(File.ReadAllText(generatedFile)));
        }
    }

    [Fact]
    public void Avalonia_extensions_are_generated_internal_by_default_without_custom_control_extensions()
    {
        var result = RunGenerator(CreateCompilation("NoGenerationAttributes", CreateSource(string.Empty, """
            public static class Usage
            {
                public static Button SetWidth() => new Button().Width(120);
            }
            """)));

        AssertNoErrors(result.OutputCompilation);
        Assert.Contains(result.GeneratedSources, source =>
            source.Contains("internal static partial class Avalonia_Controls_Button_MarkupExtensions", StringComparison.Ordinal));
        Assert.DoesNotContain(result.GeneratedSources, source =>
            source.Contains("MarkupExtensionGeneratorFixture_FixtureControl_MarkupExtensions", StringComparison.Ordinal));
    }

    [Fact]
    public void Binding_valued_Avalonia_properties_generate_value_and_compiled_binding_setters()
    {
        var result = RunGenerator(CreateCompilation(
            "BindingValuedAvaloniaProperties",
            CreateSource("[assembly: GenerateMarkupExtensionsForAvalonia]", string.Empty)));

        AssertNoErrors(result.OutputCompilation);

        var properties = new[]
        {
            (Control: "AutoCompleteBox", Property: "ValueMemberBinding"),
            (Control: "ItemsControl", Property: "DisplayMemberBinding"),
            (Control: "Primitives_SelectingItemsControl", Property: "SelectedValueBinding"),
            (Control: "TableViewColumn", Property: "Binding")
        };

        foreach (var (control, property) in properties)
        {
            var generated = Assert.Single(result.GeneratedSources, source =>
                source.Contains($"internal static partial class Avalonia_Controls_{control}_MarkupExtensions", StringComparison.Ordinal));

            Assert.Contains(
                $"public static T {property}<T>(this T control, global::Avalonia.Data.BindingBase? value",
                generated,
                StringComparison.Ordinal);
            Assert.Contains(
                $"public static T {property}<T, TViewModel, TValue>(this T control, TViewModel source, Expression<Func<TViewModel, TValue>> getter",
                generated,
                StringComparison.Ordinal);
            Assert.Contains(
                $"public static Style<T> {property}<T>(this Style<T> style, global::Avalonia.Data.BindingBase? value",
                generated,
                StringComparison.Ordinal);
            Assert.Contains(
                $"public static Style<T> {property}<T, TViewModel, TValue>(this Style<T> style, TViewModel source, Expression<Func<TViewModel, TValue>> getter",
                generated,
                StringComparison.Ordinal);
            Assert.Contains(
                $"public static Style<T> {property}<T, TViewModel, TValue>(this Style<T> style, Expression<Func<TViewModel, TValue>> getter",
                generated,
                StringComparison.Ordinal);
        }
    }

    [Fact]
    public void Added_owner_property_generates_a_derived_control_extension()
    {
        const string source = """
            using Avalonia;
            using Avalonia.Controls;
            using CSharpUI.Avalonia;

            [assembly: GenerateMarkupExtensionsForAssembly(typeof(InheritanceFixture.BaseControl))]

            namespace InheritanceFixture
            {
                public class BaseControl : Control
                {
                    public static readonly StyledProperty<int> ValueProperty =
                        AvaloniaProperty.Register<BaseControl, int>(nameof(Value));

                    public virtual int Value
                    {
                        get => GetValue(ValueProperty);
                        set => SetValue(ValueProperty, value);
                    }
                }

                public sealed class AddedOwnerControl : BaseControl
                {
                    public new static readonly StyledProperty<int> ValueProperty =
                        BaseControl.ValueProperty.AddOwner<AddedOwnerControl>();

                    public new int Value
                    {
                        get => GetValue(ValueProperty);
                        set => SetValue(ValueProperty, value);
                    }
                }

                public static class Usage
                {
                    public static AddedOwnerControl SetValue(AddedOwnerControl control) => control.Value(42);
                }
            }
            """;
        var result = RunGenerator(CreateCompilation("AddedOwnerControlExtensions", source));

        AssertNoErrors(result.OutputCompilation);
        Assert.Contains(result.GeneratedSources, generated =>
            generated.Contains("public static T Value<T>", StringComparison.Ordinal) &&
            generated.Contains("where T : global::InheritanceFixture.BaseControl", StringComparison.Ordinal));
        Assert.Contains(result.GeneratedSources, generated =>
            generated.Contains(
                "Value(this global::InheritanceFixture.AddedOwnerControl control, global::System.Int32 value",
                StringComparison.Ordinal));
    }

    [Fact]
    public void Generated_factories_skip_inaccessible_external_constructors()
    {
        var controls = CreateCompilation("ExternalFactoryControls", """
            using Avalonia.Controls;

            namespace ExternalFactoryFixture;

            public class InternalConstructorControl : Control
            {
                internal InternalConstructorControl() { }
            }

            public class PublicConstructorControl : Control
            {
                public PublicConstructorControl() { }
            }
            """);
        var controlsReference = EmitReference(controls);
        var source = """
            using CSharpUI.Avalonia;
            using ExternalFactoryFixture;

            [assembly: GenerateMarkupExtensionsForAssembly(typeof(InternalConstructorControl))]
            """;
        var compilation = CreateCompilationWithReferences("ExternalFactoryConsumer", source, controlsReference);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            [new ControlFactoryGenerator().AsSourceGenerator()],
            parseOptions: new CSharpParseOptions(LanguageVersion.Preview));

        driver = driver.RunGeneratorsAndUpdateCompilation(
            compilation,
            out var outputCompilation,
            out var driverDiagnostics,
            TestContext.Current.CancellationToken);
        var runResult = driver.GetRunResult();
        var generated = runResult.Results
            .SelectMany(static result => result.GeneratedSources)
            .Select(static sourceResult => sourceResult.SourceText.ToString())
            .ToImmutableArray();

        Assert.Empty(driverDiagnostics.Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error));
        Assert.Empty(runResult.Diagnostics.Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error));
        AssertNoErrors((CSharpCompilation)outputCompilation);
        var factorySource = Assert.Single(generated, static sourceText =>
            sourceText.Contains("internal static class GeneratedControlFactories", StringComparison.Ordinal));
        Assert.Contains("PublicConstructorControl", factorySource, StringComparison.Ordinal);
        Assert.DoesNotContain("InternalConstructorControl(", factorySource, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void Avalonia_and_custom_control_options_generate_compilable_extensions(
        bool avaloniaExtensionsArePublic,
        bool customControlExtensionsArePublic)
    {
        var avaloniaAttribute = avaloniaExtensionsArePublic
            ? "[assembly: GenerateMarkupExtensionsForAvalonia(generatePublicExtensions: true)]"
            : "[assembly: GenerateMarkupExtensionsForAvalonia]";
        var customAttribute = customControlExtensionsArePublic
            ? "[assembly: GenerateMarkupExtensionsForAssembly(typeof(MarkupExtensionGeneratorFixture.FixtureControl), generatePublicExtensions: true)]"
            : "[assembly: GenerateMarkupExtensionsForAssembly(typeof(MarkupExtensionGeneratorFixture.FixtureControl))]";
        var result = RunGenerator(CreateCompilation($"Accessibility_{avaloniaExtensionsArePublic}_{customControlExtensionsArePublic}", CreateSource($"{avaloniaAttribute}\n{customAttribute}", """
            public static class Usage
            {
                public static Button SetWidth() => new Button().Width(123);
                public static FixtureControl SetValue() => new FixtureControl().Value(42);
            }
            """)));

        AssertNoErrors(result.OutputCompilation);
        Assert.Contains(result.GeneratedSources, source => source.Contains(
            $"{(avaloniaExtensionsArePublic ? "public" : "internal")} static partial class Avalonia_Controls_Button_MarkupExtensions",
            StringComparison.Ordinal));
        Assert.Contains(result.GeneratedSources, source => source.Contains(
            $"{(customControlExtensionsArePublic ? "public" : "internal")} static partial class {FixtureNamespace}_FixtureControl_MarkupExtensions",
            StringComparison.Ordinal));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Assembly_specific_attribute_generates_custom_control_extensions(bool generatePublicExtensions)
    {
        var attribute = generatePublicExtensions
            ? "[assembly: GenerateMarkupExtensionsForAssembly(typeof(MarkupExtensionGeneratorFixture.FixtureControl), generatePublicExtensions: true)]"
            : "[assembly: GenerateMarkupExtensionsForAssembly(typeof(MarkupExtensionGeneratorFixture.FixtureControl))]";
        var result = RunGenerator(CreateCompilation("AssemblySpecific_" + generatePublicExtensions, CreateSource(attribute, """
            public static class Usage
            {
                public static FixtureControl SetValue() => new FixtureControl().Value(42);
            }
            """)));

        AssertNoErrors(result.OutputCompilation);
        Assert.Contains(result.GeneratedSources, source => source.Contains(
            $"{(generatePublicExtensions ? "public" : "internal")} static partial class {FixtureNamespace}_FixtureControl_MarkupExtensions",
            StringComparison.Ordinal));
        Assert.Contains(result.GeneratedSources, source => source.Contains(
            "internal static partial class Avalonia_Controls_Button_MarkupExtensions",
            StringComparison.Ordinal));
    }

    [Fact]
    public void Public_generated_extensions_compile_and_are_consumed_by_a_referencing_compilation()
    {
        var librarySource = CreateSource(
            """
            [assembly: GenerateMarkupExtensionsForAvalonia(generatePublicExtensions: true)]
            [assembly: GenerateMarkupExtensionsForAssembly(typeof(MarkupExtensionGeneratorFixture.FixtureControl), generatePublicExtensions: true)]
            """,
            "");
        var libraryResult = RunGenerator(CreateCompilation("PublicExtensionsLibrary", librarySource));
        AssertNoErrors(libraryResult.OutputCompilation);

        using var libraryStream = new MemoryStream();
        var emitResult = libraryResult.OutputCompilation.Emit(libraryStream, cancellationToken: TestContext.Current.CancellationToken);
        Assert.True(emitResult.Success, string.Join(Environment.NewLine, emitResult.Diagnostics));

        var referencedLibrary = MetadataReference.CreateFromImage(ImmutableArray.CreateRange(libraryStream.ToArray()));
        var consumerSource = """
            using Avalonia.Controls;
            using CSharpUI.Avalonia;
            using MarkupExtensionGeneratorFixture;

            public static class Consumer
            {
                public static Button SetWidth() => new Button().Width(240);
                public static FixtureControl SetValue() => new FixtureControl().Value(99);
            }
            """;
        var consumerResult = RunGenerator(CreateCompilation("PublicExtensionsConsumer", consumerSource, referencedLibrary));

        AssertNoErrors(consumerResult.OutputCompilation);
        Assert.DoesNotContain(consumerResult.GeneratedSources, source =>
            source.Contains("Avalonia_Controls_Button_MarkupExtensions", StringComparison.Ordinal));
        Assert.DoesNotContain(consumerResult.GeneratedSources, source =>
            source.Contains($"{FixtureNamespace}_FixtureControl_MarkupExtensions", StringComparison.Ordinal));
    }

    [Fact]
    public void Multiple_referenced_public_Avalonia_extension_providers_report_a_warning()
    {
        var firstProvider = CreatePublicAvaloniaExtensionsReference("FirstPublicExtensionsProvider");
        var secondProvider = CreatePublicAvaloniaExtensionsReference("SecondPublicExtensionsProvider");
        var consumer = RunGenerator(CreateCompilationWithReferences(
            "MultiplePublicExtensionsConsumer",
            CreateSource(string.Empty, """
                public static class ConsumerUsage
                {
                    public static Button SetWidth() => new Button().Width(120);
                }
                """),
            firstProvider,
            secondProvider));

        var warning = Assert.Single(consumer.Diagnostics.Where(static diagnostic =>
            diagnostic.Id == "CSUIGEN001"));

        Assert.Equal(DiagnosticSeverity.Warning, warning.Severity);
        Assert.Contains("Avalonia.Controls.Button", warning.GetMessage(), StringComparison.Ordinal);
        Assert.Contains("FirstPublicExtensionsProvider", warning.GetMessage(), StringComparison.Ordinal);
        Assert.Contains("SecondPublicExtensionsProvider", warning.GetMessage(), StringComparison.Ordinal);
        Assert.DoesNotContain(consumer.GeneratedSources, static source =>
            source.Contains("Avalonia_Controls_Button_MarkupExtensions", StringComparison.Ordinal));
        Assert.Contains(
            consumer.OutputCompilation.GetDiagnostics(TestContext.Current.CancellationToken),
            static diagnostic => diagnostic.Id == "CS0121");
    }

    [Fact]
    public void Single_referenced_public_Avalonia_extension_provider_does_not_report_a_warning()
    {
        var provider = CreatePublicAvaloniaExtensionsReference("SinglePublicExtensionsProvider");
        var consumer = RunGenerator(CreateCompilationWithReferences(
            "SinglePublicExtensionsConsumer",
            CreateSource(string.Empty, ""),
            provider));

        Assert.DoesNotContain(consumer.Diagnostics, static diagnostic => diagnostic.Id == "CSUIGEN001");
    }

    [Fact]
    public void Same_named_handwritten_extension_classes_do_not_report_a_warning()
    {
        const string handwrittenExtensions = """
            using Avalonia.Controls;

            namespace CSharpUI.Avalonia
            {
                public static class Avalonia_Controls_Button_MarkupExtensions
                {
                    public static Button Width(this Button button, double width) => button;
                }
            }
            """;
        var firstProvider = EmitReference(CreateCompilation("HandwrittenProviderOne", handwrittenExtensions));
        var secondProvider = EmitReference(CreateCompilation("HandwrittenProviderTwo", handwrittenExtensions));
        var consumer = RunGenerator(CreateCompilationWithReferences(
            "HandwrittenExtensionsConsumer",
            CreateSource(string.Empty, ""),
            firstProvider,
            secondProvider));

        Assert.DoesNotContain(consumer.Diagnostics, static diagnostic => diagnostic.Id == "CSUIGEN001");
    }

    [Theory]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public void Options_for_the_same_Avalonia_assembly_merge_to_public(
        bool avaloniaAttributeIsPublic,
        bool assemblyAttributeIsPublic)
    {
        var avaloniaAttribute = avaloniaAttributeIsPublic
            ? "[assembly: GenerateMarkupExtensionsForAvalonia(generatePublicExtensions: true)]"
            : "[assembly: GenerateMarkupExtensionsForAvalonia]";
        var assemblyAttribute = assemblyAttributeIsPublic
            ? "[assembly: GenerateMarkupExtensionsForAssembly(typeof(Button), generatePublicExtensions: true)]"
            : "[assembly: GenerateMarkupExtensionsForAssembly(typeof(Button))]";
        var result = RunGenerator(CreateCompilation(
            $"MergeOptions_{avaloniaAttributeIsPublic}_{assemblyAttributeIsPublic}",
            CreateSource($"{avaloniaAttribute}\n{assemblyAttribute}", """
                public static class Usage
                {
                    public static Button SetWidth() => new Button().Width(123);
                }
                """)));

        AssertNoErrors(result.OutputCompilation);
        Assert.Contains(result.GeneratedSources, source => source.Contains(
            "public static partial class Avalonia_Controls_Button_MarkupExtensions",
            StringComparison.Ordinal));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Assembly_attribute_can_target_a_control_from_a_referenced_assembly(bool generatePublicExtensions)
    {
        var controlLibrary = CreateCompilation("ReferencedControls", CreateSource(string.Empty, ""));
        var controlLibraryReference = EmitReference(controlLibrary);
        var generatePublicOption = generatePublicExtensions ? ", generatePublicExtensions: true" : string.Empty;
        var consumerSource = $$"""
            using CSharpUI.Avalonia;
            using MarkupExtensionGeneratorFixture;

            [assembly: GenerateMarkupExtensionsForAssembly(typeof(FixtureControl){{generatePublicOption}})]

            public static class Usage
            {
                public static FixtureControl SetValue() => new FixtureControl().Value(42);
            }
            """;
        var result = RunGenerator(CreateCompilation("ExternalAssemblyTarget_" + generatePublicExtensions, consumerSource, controlLibraryReference));

        AssertNoErrors(result.OutputCompilation);
        Assert.Contains(result.GeneratedSources, source => source.Contains(
            $"{(generatePublicExtensions ? "public" : "internal")} static partial class {FixtureNamespace}_FixtureControl_MarkupExtensions",
            StringComparison.Ordinal));
    }

    [Fact]
    public void Referenced_internal_extensions_do_not_suppress_local_generation()
    {
        var internalLibrary = RunGenerator(CreateCompilation(
            "InternalAvaloniaExtensionsLibrary",
            CreateSource("[assembly: GenerateMarkupExtensionsForAvalonia]", "")));
        AssertNoErrors(internalLibrary.OutputCompilation);
        var internalLibraryReference = EmitReference(internalLibrary.OutputCompilation);
        var consumerSource = CreateSource(string.Empty, """
            public static class Usage
            {
                public static Button SetWidth() => new Button().Width(240);
            }
            """);
        var consumer = RunGenerator(CreateCompilation("InternalExtensionsConsumer", consumerSource, internalLibraryReference));

        AssertNoErrors(consumer.OutputCompilation);
        Assert.Contains(consumer.GeneratedSources, source => source.Contains(
            "internal static partial class Avalonia_Controls_Button_MarkupExtensions",
            StringComparison.Ordinal));
    }

    [Fact]
    public void Public_generated_extensions_disable_missing_documentation_warnings()
    {
        var source = CreateSource(
            "[assembly: GenerateMarkupExtensionsForAvalonia(generatePublicExtensions: true)]",
            "");
        var compilation = CreateCompilation(
            "DocumentedPublicExtensions",
            source,
            compilationOptions: new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                generalDiagnosticOption: ReportDiagnostic.Error,
                nullableContextOptions: NullableContextOptions.Enable)
                .WithSpecificDiagnosticOptions(ImmutableDictionary<string, ReportDiagnostic>.Empty
                    .Add("CS1701", ReportDiagnostic.Suppress)));
        var result = RunGenerator(compilation);

        using var assemblyStream = new MemoryStream();
        using var documentationStream = new MemoryStream();
        var emitResult = result.OutputCompilation.Emit(
            assemblyStream,
            xmlDocumentationStream: documentationStream,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Contains(result.GeneratedSources, generated => generated.Contains("#pragma warning disable CS1591", StringComparison.Ordinal));
        Assert.True(emitResult.Success, string.Join(Environment.NewLine, emitResult.Diagnostics));
    }

    [Fact]
    public void Generation_attribute_constructors_preserve_options()
    {
        var constructor = typeof(GenerateMarkupExtensionsForAssemblyAttribute).GetConstructor([typeof(Type)]);
        var internalAttribute = new GenerateMarkupExtensionsForAssemblyAttribute(typeof(Control));
        var publicAttribute = new GenerateMarkupExtensionsForAssemblyAttribute(typeof(Control), generatePublicExtensions: true);
        var avaloniaAttribute = new GenerateMarkupExtensionsForAvaloniaAttribute(generatePublicExtensions: true);

        Assert.NotNull(constructor);
        Assert.False(internalAttribute.GeneratePublicExtensions);
        Assert.True(publicAttribute.GeneratePublicExtensions);
        Assert.True(avaloniaAttribute.GeneratePublicExtensions);
    }

    private static GeneratorResult RunGenerator(CSharpCompilation compilation)
    {
        var parseOptions = new CSharpParseOptions(LanguageVersion.Preview);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            [new ExternalAssemblyExtensionsGenerator().AsSourceGenerator()],
            parseOptions: parseOptions);
        driver = driver.RunGeneratorsAndUpdateCompilation(
            compilation,
            out var outputCompilation,
            out var driverDiagnostics,
            TestContext.Current.CancellationToken);
        var runResult = driver.GetRunResult();
        var generatedSources = runResult.Results
            .SelectMany(static result => result.GeneratedSources)
            .Select(static generated => generated.SourceText.ToString())
            .ToImmutableArray();

        Assert.Empty(driverDiagnostics.Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error));
        Assert.Empty(runResult.Diagnostics.Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error));
        return new GeneratorResult(
            (CSharpCompilation)outputCompilation,
            generatedSources,
            driverDiagnostics);
    }

    private static PortableExecutableReference CreatePublicAvaloniaExtensionsReference(string assemblyName)
    {
        var libraryResult = RunGenerator(CreateCompilation(
            assemblyName,
            CreateSource("[assembly: GenerateMarkupExtensionsForAvalonia(generatePublicExtensions: true)]", "")));
        AssertNoErrors(libraryResult.OutputCompilation);
        return EmitReference(libraryResult.OutputCompilation);
    }

    private static PortableExecutableReference EmitReference(CSharpCompilation compilation)
    {
        using var assemblyStream = new MemoryStream();
        var emitResult = compilation.Emit(assemblyStream, cancellationToken: TestContext.Current.CancellationToken);
        Assert.True(emitResult.Success, string.Join(Environment.NewLine, emitResult.Diagnostics));
        return MetadataReference.CreateFromImage(ImmutableArray.CreateRange(assemblyStream.ToArray()));
    }

    private static CSharpCompilation CreateCompilation(
        string assemblyName,
        string source,
        MetadataReference? additionalReference = null,
        CSharpCompilationOptions? compilationOptions = null)
    {
        var references = additionalReference is null
            ? PlatformReferences
            : PlatformReferences.Add(additionalReference);

        return CSharpCompilation.Create(
            assemblyName,
            [CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview))],
            references,
            compilationOptions ?? new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable));
    }

    private static CSharpCompilation CreateCompilationWithReferences(
        string assemblyName,
        string source,
        params MetadataReference[] additionalReferences) =>
        CSharpCompilation.Create(
            assemblyName,
            [CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview))],
            PlatformReferences.AddRange(additionalReferences),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable));

    private static ImmutableArray<MetadataReference> CreatePlatformReferences()
    {
        var frameworkVersion = Version.Parse(AppContext.TargetFrameworkName![(AppContext.TargetFrameworkName!.LastIndexOf('v') + 1)..]);
        var dotnetRoot = Path.GetFullPath(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "..", "..", ".."));
        var referencePacksDirectory = Path.Combine(dotnetRoot, "packs", "Microsoft.NETCore.App.Ref");
        var referencePack = Directory.EnumerateDirectories(referencePacksDirectory)
            .Where(directory => Path.GetFileName(directory).StartsWith($"{frameworkVersion.Major}.", StringComparison.Ordinal))
            .OrderByDescending(directory => Path.GetFileName(directory), StringComparer.Ordinal)
            .FirstOrDefault()
            ?? throw new InvalidOperationException($"No .NET {frameworkVersion.Major} reference pack was found under '{referencePacksDirectory}'.");
        var frameworkReferences = Directory.EnumerateFiles(
                Path.Combine(referencePack, "ref", $"net{frameworkVersion.Major}.0"), "*.dll")
            .Select(static path => MetadataReference.CreateFromFile(path));

        return [
            .. frameworkReferences,
            MetadataReference.CreateFromFile(typeof(Control).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(AvaloniaProperty).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(GenerateMarkupExtensionsForAvaloniaAttribute).Assembly.Location)
        ];
    }

    private static string CreateSource(string assemblyAttributes, string usage) => $$"""
        using Avalonia;
        using Avalonia.Controls;
        using CSharpUI.Avalonia;

        {{assemblyAttributes}}

        namespace {{FixtureNamespace}}
        {
            /// <summary>A control used by markup extension generator tests.</summary>
            public class FixtureControl : Control
            {
                /// <summary>The fixture control value property.</summary>
                public static readonly StyledProperty<int> ValueProperty =
                    AvaloniaProperty.Register<FixtureControl, int>(nameof(Value));

                /// <summary>The fixture control value.</summary>
                public int Value
                {
                    get => GetValue(ValueProperty);
                    set => SetValue(ValueProperty, value);
                }
            }
        }

        namespace {{FixtureNamespace}}
        {
        {{usage}}
        }
        """;

    private static string NormalizeGeneratedSource(string source) =>
        string.Join(
            "\n",
            source.Replace("\r\n", "\n", StringComparison.Ordinal)
                .Split('\n')
                .Select(static line => line.TrimEnd()))
        .TrimEnd('\n');

    private static void AssertNoErrors(CSharpCompilation compilation)
    {
        var errors = compilation.GetDiagnostics().Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
        Assert.Empty(errors);
    }

    private sealed record GeneratorResult(
        CSharpCompilation OutputCompilation,
        ImmutableArray<string> GeneratedSources,
        ImmutableArray<Diagnostic> Diagnostics);
}

