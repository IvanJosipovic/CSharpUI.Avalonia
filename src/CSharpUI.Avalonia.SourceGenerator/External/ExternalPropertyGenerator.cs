using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Diagnostics;

namespace CSharpUI.Avalonia.SourceGenerator.External;

[Generator]
public class ExternalPropertyGenerator : SourceGeneratorBase, IIncrementalGenerator
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
        foreach (var publicClass in GetPublicClasses(symbol.GlobalNamespace))
        {
            if (InheritsFrom(publicClass, "Avalonia.Controls.Control"))
            {
                GenerateSource(spc, publicClass);
            }
        }
    }
}