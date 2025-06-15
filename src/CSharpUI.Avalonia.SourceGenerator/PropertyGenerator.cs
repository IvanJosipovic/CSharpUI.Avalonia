using CSharpUI.Avalonia.SourceGenerator.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace CSharpUI.Avalonia.SourceGenerator;

[Generator]
public class PropertyGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
#if DEBUG
        if (!Debugger.IsAttached)
        {
            //Debugger.Launch();
        }
#endif

        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => s is ClassDeclarationSyntax,
                transform: static (ctx, _) => GetSemanticTarget(ctx))
            .Where(static c => c is not null);

        context.RegisterSourceOutput(classDeclarations,
            static (spc, data) =>
            {
                if (Extensions.InheritsFrom(data!, "Avalonia.Controls.Control"))
                {
                    var generator = new GeneratorHost();

                    var code = generator.GenerateExtensions(data!);

                    if (code != null)
                    {
                        spc.AddSource($"{Extensions.RemoveIllegalFileNameCharacters(data!.ToString())}.g.cs", code);
                    }
                }
            });
    }

    private static INamedTypeSymbol? GetSemanticTarget(GeneratorSyntaxContext context)
    {
        var classDecl = (ClassDeclarationSyntax)context.Node;
        var symbol = context.SemanticModel.GetDeclaredSymbol(classDecl);

        if (symbol != null && Extensions.InheritsFrom(symbol, "Avalonia.Controls.Control"))
        {
            return symbol!;
        }

        return null;
    }
}