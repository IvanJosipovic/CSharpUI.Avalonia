using System.Text;
using CSharpUI.Avalonia.SourceGenerator.Analysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CSharpUI.Avalonia.SourceGenerator.InternalGenerators;

public sealed class ViewFactoryRegistrationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var factoryTargets = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: static (ctx, _) => GetFactoryTarget(ctx))
            .Where(static target => target is not null)
            .Select(static (target, _) => target!.Value)
            .WithComparer(ViewFactoryRegistrationTargetComparer.Instance);

        context.RegisterSourceOutput(factoryTargets, static (spc, target) => GenerateSource(spc, target));
    }

    private static ViewFactoryRegistrationTarget? GetFactoryTarget(GeneratorSyntaxContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclaration)
        {
            return null;
        }

        if (context.SemanticModel.GetDeclaredSymbol(classDeclaration) is not INamedTypeSymbol typeSymbol)
        {
            return null;
        }

        var model = ControlModelBuilder.Build(typeSymbol, context.SemanticModel.Compilation.Assembly);
        if (!IsEligibleControlType(model))
        {
            return null;
        }

        return ViewFactoryRegistrationTarget.Create(model.Type);
    }

    private static bool IsEligibleControlType(ControlModel model) =>
        model.Type.TypeKind == TypeKind.Class &&
        !model.IgnoreGeneratedExtensions &&
        model.GenerateFactory &&
        model.IsConstructible &&
        model.Type.IsOrInheritsFrom("Avalonia.Controls.Control") &&
        model.Type.IsAccessibleFromGeneratedCode();

    private static void GenerateSource(SourceProductionContext context, ViewFactoryRegistrationTarget target)
    {
        var sb = new StringBuilder(256);
        sb.AppendLine("#nullable enable");
        sb.AppendLine("using System.Runtime.CompilerServices;");
        sb.AppendLine("namespace CSharpUI.Avalonia;");
        sb.AppendLine($"internal static class {target.RegistrationClassName}");
        sb.AppendLine("{");
        sb.AppendLine("    [ModuleInitializer]");
        sb.AppendLine("    internal static void Register()");
        sb.AppendLine("    {");
        sb.AppendLine($"        global::CSharpUI.Avalonia.ViewFactoryRegistry.Register<{target.ControlTypeName}>(static () => new {target.ControlTypeName}());");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        context.AddSource(target.HintName, SourceText.From(sb.ToString(), Encoding.UTF8));
    }

    private readonly struct ViewFactoryRegistrationTarget
    {
        public ViewFactoryRegistrationTarget(string controlTypeName, string registrationClassName, string hintName)
        {
            ControlTypeName = controlTypeName;
            RegistrationClassName = registrationClassName;
            HintName = hintName;
        }

        public string ControlTypeName { get; }

        public string RegistrationClassName { get; }

        public string HintName { get; }

        public static ViewFactoryRegistrationTarget Create(INamedTypeSymbol typeSymbol)
        {
            var typeIdentifier = SymbolUtilities.BuildTypeIdentifier(typeSymbol);
            var registrationClassName = $"{typeIdentifier}_ViewFactoryRegistration";
            var hintName = SymbolUtilities.RemoveIllegalFileNameCharacters(registrationClassName) + ".g.cs";

            return new ViewFactoryRegistrationTarget(typeSymbol.GetFullTypeName(), registrationClassName, hintName);
        }
    }

    private sealed class ViewFactoryRegistrationTargetComparer : IEqualityComparer<ViewFactoryRegistrationTarget>
    {
        internal static readonly ViewFactoryRegistrationTargetComparer Instance = new();

        public bool Equals(ViewFactoryRegistrationTarget x, ViewFactoryRegistrationTarget y) =>
            StringComparer.Ordinal.Equals(x.HintName, y.HintName);

        public int GetHashCode(ViewFactoryRegistrationTarget obj) =>
            StringComparer.Ordinal.GetHashCode(obj.HintName);
    }
}
