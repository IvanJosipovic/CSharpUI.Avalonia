using CSharpUI.Avalonia.SourceGenerator.ExternalGenerators;
using CSharpUI.Avalonia.SourceGenerator.InternalGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace CSharpUI.Avalonia.SourceGenerator;

/// <summary>
/// Compatibility entry point for tests and direct generator hosts.
/// </summary>
[Generator]
public sealed class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        new ExternalAssemblyExtensionsGenerator().Initialize(context);
        new LocalControlExtensionsGenerator().Initialize(context);
        new AvaloniaPropertyExtensionsGenerator().Initialize(context);
        new AvaloniaEventExtensionsGenerator().Initialize(context);
        new ViewFactoryRegistrationGenerator().Initialize(context);
        new ControlFactoryGenerator().Initialize(context);

        context.RegisterPostInitializationOutput(static output => output.AddSource(
            "CSharpUIGlobalUsings.g.cs",
            SourceText.From(
                "global using CSharpUI.Avalonia;\n" +
                "global using CSharpUI.Avalonia.CommonExtensions;\n",
                Encoding.UTF8)));
    }
}
