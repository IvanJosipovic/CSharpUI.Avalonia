using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Diagnostics;

namespace CSharpUI.Avalonia.SourceGenerator;

[Generator]
public class ExtensionAvaloniaPropertyExtensionsGenerator : IIncrementalGenerator
{

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
#if DEBUG
        if (!Debugger.IsAttached)
        {
            //Debugger.Launch();
        }
#endif
        Debug.WriteLine("Initialize ExtensionAvaloniaPropertyExtensionsGenerator code generator");

        var attribute = context.SyntaxProvider
                         .ForAttributeWithMetadataName("CSharpUI.Avalonia.GenerateExtensionsForAssemblyAttribute",
                                                       static (s, _) => true,
                                                       static (ctx, _) => GetSemanticTarget(ctx))
                         .Collect()
                         .SelectMany((enumInfos, _) => enumInfos.Distinct());


        context.RegisterSourceOutput(attribute,
            static (spc, data) => GenerateSource(spc, data!.Value));
    }

    private static void GenerateSource(SourceProductionContext spc, ImmutableArray<IAssemblySymbol>? assemblies)
    {
        throw new NotImplementedException();
    }

    private static ImmutableArray<IAssemblySymbol>? GetSemanticTarget(GeneratorAttributeSyntaxContext context)
    {
        var assemblies = ImmutableArray.CreateBuilder<IAssemblySymbol>();

        foreach (var attribute in context.Attributes)
        {
            if (attribute?.AttributeClass?.Name == "GenerateExtensionsForAssemblyAttribute" && attribute.ConstructorArguments[0].Value is INamedTypeSymbol iNamedTypeSymbol)
                assemblies.Add(iNamedTypeSymbol.ContainingAssembly);
        }

        return assemblies.ToImmutable();
    }
}