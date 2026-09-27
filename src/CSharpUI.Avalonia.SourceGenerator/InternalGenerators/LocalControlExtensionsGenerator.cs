using System.Collections.Immutable;
using System.Text;
using CSharpUI.Avalonia.SourceGenerator.Analysis;
using CSharpUI.Avalonia.SourceGenerator.ExternalGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CSharpUI.Avalonia.SourceGenerator.InternalGenerators;

public sealed class LocalControlExtensionsGenerator : IIncrementalGenerator
{
    private static readonly ExternalGeneratorHost GeneratorHost = new();

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var localModels = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: static (syntaxContext, _) => GetControlModel(syntaxContext))
            .Where(static model => model is not null)
            .Select(static (model, _) => model!)
            .Collect();

        var generationInput = localModels.Combine(context.CompilationProvider);
        context.RegisterSourceOutput(generationInput, static (sourceContext, input) =>
            Generate(sourceContext, input.Left, input.Right));
    }

    private static ControlModel? GetControlModel(GeneratorSyntaxContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclaration ||
            context.SemanticModel.GetDeclaredSymbol(classDeclaration) is not INamedTypeSymbol type ||
            !type.IsOrInheritsFrom("Avalonia.AvaloniaObject") ||
            !type.IsAccessibleFromGeneratedCode())
        {
            return null;
        }

        return ControlModelBuilder.Build(type, context.SemanticModel.Compilation.Assembly);
    }

    private static void Generate(
        SourceProductionContext context,
        ImmutableArray<ControlModel> models,
        Compilation compilation)
    {
        var explicitTargets = SymbolUtilities.GetTargetAssemblies(compilation)
            .Select(static target => target.Assembly)
            .ToArray();
        var emittedTypes = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        foreach (var model in models)
        {
            if (!emittedTypes.Add(model.Type) ||
                model.IgnoreGeneratedExtensions ||
                explicitTargets.Any(target => SymbolEqualityComparer.Default.Equals(target, model.Type.ContainingAssembly)))
            {
                continue;
            }

            try
            {
                var code = GeneratorHost.GenerateExtensions(model, generatePublicExtensions: false);
                if (string.IsNullOrWhiteSpace(code))
                {
                    continue;
                }

                var typeName = model.Type.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
                var hintName = $"Local_{SymbolUtilities.RemoveIllegalFileNameCharacters(typeName)}.g.cs";
                context.AddSource(hintName, SourceText.From(code!, Encoding.UTF8));
            }
            catch (Exception exception)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    ExternalAssemblyExtensionsGenerator.ExtensionGenerationFailed,
                    model.Type.Locations.FirstOrDefault(),
                    model.Type.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat),
                    exception.Message));
            }
        }
    }
}
