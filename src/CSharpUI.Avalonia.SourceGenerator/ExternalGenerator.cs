using CSharpUI.Avalonia.SourceGenerator.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace CSharpUI.Avalonia.SourceGenerator;

[Generator]
public class ExternalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
#if DEBUG
        if (!Debugger.IsAttached)
        {
            //Debugger.Launch();
        }
#endif
        var attribute = context.SyntaxProvider
                         .ForAttributeWithMetadataName("CSharpUI.Avalonia.GenerateExtensionsForAssemblyAttribute",
                                                       static (s, _) => true,
                                                       static (ctx, _) => GetSemanticTarget(ctx))
                         .SelectMany((assemblies, _) => assemblies)
                         .Collect()
                         .Select((assemblies, _) => assemblies.Distinct<IAssemblySymbol>(SymbolEqualityComparer.Default)
                                                              .ToImmutableArray());

            context.RegisterSourceOutput(attribute,
                static (spc, assemblies) =>
                {
                    foreach (var assembly in assemblies)
                    {
                        GetClasses(spc, assembly);
                    }
                });
    }

    private static ImmutableArray<IAssemblySymbol> GetSemanticTarget(GeneratorAttributeSyntaxContext context)
    {
        var assemblies = new HashSet<IAssemblySymbol>(SymbolEqualityComparer.Default);

        foreach (var attribute in context.Attributes)
        {
            if (attribute?.AttributeClass?.Name == "GenerateExtensionsForAssemblyAttribute" &&
                attribute.ConstructorArguments.Length > 0 &&
                attribute.ConstructorArguments[0].Value is INamedTypeSymbol iNamedTypeSymbol)
            {
                assemblies.Add(iNamedTypeSymbol.ContainingAssembly);
            }
        }

        return [.. assemblies];
    }

    private static void GetClasses(SourceProductionContext spc, IAssemblySymbol symbol)
    {
        var generator = new GeneratorHost();

        foreach (var publicClass in Extensions.GetPublicClasses(symbol.GlobalNamespace))
        {
            if (Extensions.InheritsFrom(publicClass, "Avalonia.Controls.Control"))
            {
                var code = generator.GenerateExtensions(publicClass);

                if (code != null)
                {
                    spc.AddSource($"{Extensions.RemoveIllegalFileNameCharacters(publicClass.ToString())}.g.cs", code);
                }
            }
        }
    }
}