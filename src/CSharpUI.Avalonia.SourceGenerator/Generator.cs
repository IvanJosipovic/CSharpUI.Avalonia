using CSharpUI.Avalonia.SourceGenerator.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Text;

namespace CSharpUI.Avalonia.SourceGenerator;

[Generator]
public class Generator : IIncrementalGenerator
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
                predicate: static (syntaxNode, _) => syntaxNode is ClassDeclarationSyntax,
                transform: static (ctx, _) => GetSemanticTarget(ctx)
            )
            .Where(static c => c is not null);

        context.RegisterSourceOutput(classDeclarations,
            static (spc, data) =>
            {
                var generator = new GeneratorHost();

                var code = generator.GenerateExtensions(data!);

                if (!string.IsNullOrEmpty(code))
                {
                    spc.AddSource($"{Extensions.RemoveIllegalFileNameCharacters(data!.ToString())}.g.cs", code!);
                }
            });

        context.RegisterPostInitializationOutput(pi =>
        {
            pi.AddSource("GlobalUsings.g.cs", SourceText.From($"global using CSharpUI.Avalonia.Extensions;\n", Encoding.UTF8));
        });
    }

    private static INamedTypeSymbol? GetSemanticTarget(GeneratorSyntaxContext context)
    {
        var classDecl = (ClassDeclarationSyntax)context.Node;
        var symbol = context.SemanticModel.GetDeclaredSymbol(classDecl);

        if (symbol != null && Extensions.InheritsFrom(symbol, "Avalonia.Visual"))
        {
            return symbol!;
        }

        return null;
    }
}