using CSharpUI.Avalonia.SourceGenerator.ExtensionInfos;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpUI.Avalonia.SourceGenerator.Analysis;

internal static class ControlModelBuilder
{
    internal static ControlModel Build(INamedTypeSymbol type, IAssemblySymbol? generatedAssembly = null)
    {
        var cache = new Dictionary<INamedTypeSymbol, ControlModel>(SymbolEqualityComparer.Default);
        return Build(type, generatedAssembly, cache);
    }

    private static ControlModel Build(
        INamedTypeSymbol type,
        IAssemblySymbol? generatedAssembly,
        Dictionary<INamedTypeSymbol, ControlModel> cache)
    {
        if (cache.TryGetValue(type, out var cached))
        {
            return cached;
        }

        var baseType = type.BaseType is { } baseSymbol
            ? Build(baseSymbol, generatedAssembly, cache)
            : null;

        var declaredProperties = BuildDeclaredProperties(type, baseType);
        var properties = MergeProperties(baseType, declaredProperties);

        var declaredEvents = BuildDeclaredEvents(type, baseType);
        var events = MergeEvents(baseType, declaredEvents);

        var declaredAttachedProperties = type.GetMembers()
            .OfType<IFieldSymbol>()
            .Where(static field => field.IsAttachedPropertyField())
            .Select(static field =>
            {
                var metadata = GetMetadata(field);
                return new AttachedPropertyModel(
                    field,
                    PropertyInheritanceKind.Declared,
                    GetOwnershipKind(field),
                    metadata.Alias ?? field.GetMemberName(),
                    metadata.Ignore);
            })
            .ToArray();
        var attachedProperties = MergeAttachedProperties(baseType, declaredAttachedProperties);

        var stylePropertyFields = GetStylePropertyFields(type);
        var constructors = type.InstanceConstructors
            .Select(constructor => new ConstructorModel(
                constructor,
                IsAccessibleFromGeneratedCode(constructor, generatedAssembly)))
            .ToArray();
        var contentSlotNames = new HashSet<string>(
            declaredProperties
                .Where(static property => property.IsContentSlot && property.ClrProperty is not null)
                .Select(static property => property.Name),
            StringComparer.Ordinal);
        var declaredContentSlots = type.GetMembers()
            .OfType<IPropertySymbol>()
            .Where(static property => property.DeclaredAccessibility == Accessibility.Public)
            .Where(property => contentSlotNames.Contains(property.Name))
            .Select(static property => new ContentSlotModel(property))
            .ToArray();
        var contentSlots = declaredContentSlots
            .Concat(baseType?.ContentSlots ?? [])
            .GroupBy(static contentSlot => contentSlot.Name, StringComparer.Ordinal)
            .Select(static group => group.First())
            .ToArray();
        var typeMetadata = GetMetadata(type);

        var model = new ControlModel(
            type,
            baseType,
            constructors,
            declaredProperties,
            properties,
            declaredEvents,
            events,
            declaredAttachedProperties,
            attachedProperties,
            stylePropertyFields,
            contentSlots,
            typeMetadata.Ignore,
            typeMetadata.GenerateFactory,
            typeMetadata.Alias ?? type.Name);
        cache.Add(type, model);
        return model;
    }

    private static PropertyModel[] MergeProperties(ControlModel? baseType, PropertyModel[] declaredProperties)
    {
        var properties = baseType?.Properties
            .Select(static property => property.AsInherited())
            .ToList() ?? [];

        foreach (var property in declaredProperties)
        {
            if (property.InheritanceKind == PropertyInheritanceKind.Override && property.ClrProperty?.OverriddenProperty is { } overridden)
            {
                var overriddenIndex = properties.FindLastIndex(candidate =>
                    candidate.ClrProperty is not null &&
                    SymbolEqualityComparer.Default.Equals(candidate.ClrProperty, overridden));
                if (overriddenIndex >= 0)
                {
                    properties.RemoveAt(overriddenIndex);
                }
            }
            else if (property.OwnershipKind is AvaloniaPropertyOwnershipKind.AddedOwner or AvaloniaPropertyOwnershipKind.MetadataOverride)
            {
                var ownedPropertyIndex = properties.FindLastIndex(candidate =>
                    StringComparer.Ordinal.Equals(candidate.Name, property.Name));
                if (ownedPropertyIndex >= 0)
                {
                    properties.RemoveAt(ownedPropertyIndex);
                }
            }

            properties.Add(property);
        }

        return properties.ToArray();
    }

    private static EventModel[] MergeEvents(ControlModel? baseType, EventModel[] declaredEvents)
    {
        var events = baseType?.Events
            .Select(static eventModel => eventModel.AsInherited())
            .ToList() ?? [];

        foreach (var eventModel in declaredEvents)
        {
            if (eventModel.InheritanceKind == PropertyInheritanceKind.Override && eventModel.Symbol.OverriddenEvent is { } overridden)
            {
                var overriddenIndex = events.FindLastIndex(candidate =>
                    SymbolEqualityComparer.Default.Equals(candidate.Symbol, overridden));
                if (overriddenIndex >= 0)
                {
                    events.RemoveAt(overriddenIndex);
                }
            }

            events.Add(eventModel);
        }

        return events.ToArray();
    }

    private static AttachedPropertyModel[] MergeAttachedProperties(
        ControlModel? baseType,
        AttachedPropertyModel[] declaredAttachedProperties)
    {
        var attachedProperties = baseType?.AttachedProperties
            .Select(static property => property.AsInherited())
            .ToList() ?? [];

        foreach (var property in declaredAttachedProperties)
        {
            if (property.OwnershipKind is AvaloniaPropertyOwnershipKind.AddedOwner or AvaloniaPropertyOwnershipKind.MetadataOverride)
            {
                var ownedPropertyIndex = attachedProperties.FindLastIndex(candidate =>
                    StringComparer.Ordinal.Equals(candidate.Name, property.Name));
                if (ownedPropertyIndex >= 0)
                {
                    attachedProperties.RemoveAt(ownedPropertyIndex);
                }
            }

            attachedProperties.Add(property);
        }

        return attachedProperties.ToArray();
    }

    private static bool IsAccessibleFromGeneratedCode(IMethodSymbol constructor, IAssemblySymbol? generatedAssembly)
    {
        if (constructor.DeclaredAccessibility == Accessibility.Public)
        {
            return true;
        }

        var isSameAssembly = generatedAssembly is not null &&
            SymbolEqualityComparer.Default.Equals(constructor.ContainingAssembly, generatedAssembly);

        return isSameAssembly &&
            constructor.DeclaredAccessibility is Accessibility.Internal or Accessibility.ProtectedOrInternal;
    }

    private static PropertyModel[] BuildDeclaredProperties(INamedTypeSymbol type, ControlModel? baseType)
    {
        var inheritedByName = baseType?.Properties
            .GroupBy(static property => property.Name, StringComparer.Ordinal)
            .ToDictionary(static group => group.Key, static group => group.First(), StringComparer.Ordinal)
            ?? new Dictionary<string, PropertyModel>(StringComparer.Ordinal);

        var declared = new Dictionary<string, PropertyModel>(StringComparer.Ordinal);
        foreach (var property in type.GetMembers().OfType<IPropertySymbol>()
                     .Where(static property => property.DeclaredAccessibility == Accessibility.Public && !property.IsStatic))
        {
            var avaloniaField = type.GetMembers(property.Name + "Property")
                .OfType<IFieldSymbol>()
                .FirstOrDefault(static field => field.IsAvaloniaPropertyField());
            var propertyMetadata = GetMetadata(property);
            var fieldMetadata = avaloniaField is null ? default : GetMetadata(avaloniaField);
            var inheritanceKind = GetInheritanceKind(property.IsOverride, inheritedByName.ContainsKey(property.Name));
            declared[property.Name] = new PropertyModel(
                property.Name,
                propertyMetadata.Alias ?? fieldMetadata.Alias ?? property.Name,
                type,
                property,
                avaloniaField,
                property.Type,
                inheritanceKind,
                avaloniaField is null ? AvaloniaPropertyOwnershipKind.Registered : GetOwnershipKind(avaloniaField),
                propertyMetadata.Ignore || fieldMetadata.Ignore,
                propertyMetadata.Content || fieldMetadata.Content || HasContentAttribute(property));
        }

        foreach (var field in type.GetMembers().OfType<IFieldSymbol>()
                     .Where(static field => field.IsAvaloniaPropertyField()))
        {
            var name = field.GetMemberName();
            if (declared.TryGetValue(name, out var existing))
            {
                if (existing.AvaloniaPropertyField is null)
                {
                    declared[name] = new PropertyModel(
                        existing.Name,
                        GetMetadata(field).Alias ?? existing.ExtensionName,
                        type,
                        existing.ClrProperty,
                        field,
                        field.Type.GetLastGenericArgument(),
                        existing.InheritanceKind,
                        GetOwnershipKind(field),
                        existing.IsIgnored || GetMetadata(field).Ignore,
                        existing.IsContentSlot || GetMetadata(field).Content);
                }

                continue;
            }

            inheritedByName.TryGetValue(name, out var inherited);
            var ownershipKind = GetOwnershipKind(field);
            declared[name] = new PropertyModel(
                name,
                GetMetadata(field).Alias ?? name,
                type,
                null,
                field,
                field.Type.GetLastGenericArgument(),
                inherited is null
                    ? PropertyInheritanceKind.Declared
                    : ownershipKind is AvaloniaPropertyOwnershipKind.AddedOwner or AvaloniaPropertyOwnershipKind.MetadataOverride
                        ? PropertyInheritanceKind.Inherited
                        : PropertyInheritanceKind.Shadow,
                ownershipKind,
                GetMetadata(field).Ignore,
                GetMetadata(field).Content);
        }

        return declared.Values.ToArray();
    }

    private static EventModel[] BuildDeclaredEvents(INamedTypeSymbol type, ControlModel? baseType)
    {
        var inheritedNames = new HashSet<string>(StringComparer.Ordinal);
        if (baseType is not null)
        {
            foreach (var inheritedEvent in baseType.Events)
            {
                inheritedNames.Add(inheritedEvent.Name);
            }
        }

        return type.GetMembers().OfType<IEventSymbol>()
            .Where(static eventSymbol => eventSymbol.DeclaredAccessibility == Accessibility.Public)
            .Select(eventSymbol =>
            {
                var metadata = GetMetadata(eventSymbol);
                return new EventModel(
                    eventSymbol,
                    GetInheritanceKind(eventSymbol.IsOverride, inheritedNames.Contains(eventSymbol.Name)),
                    metadata.Alias ?? eventSymbol.Name,
                    metadata.Ignore);
            })
            .Where(static eventModel => !eventModel.IsIgnored)
            .ToArray();
    }

    private static IReadOnlyList<IFieldSymbol> GetStylePropertyFields(INamedTypeSymbol type)
    {
        IEnumerable<IFieldSymbol> fields = type.GetMembers().OfType<IFieldSymbol>();
        if (type.GetFullTypeName() == "global::Avalonia.StyledElement")
        {
            fields = fields.Concat(type.GetAllMembers()
                .OfType<IFieldSymbol>()
                .Where(field => field.ContainingType is not null &&
                    !SymbolEqualityComparer.Default.Equals(field.ContainingType, type) &&
                    !field.ContainingType.IsOrInheritsFrom("Avalonia.StyledElement")));
        }

        return fields.Where(static field => field.IsAcceptableStyledField()).ToArray();
    }

    private static bool HasContentAttribute(IPropertySymbol property) =>
        property.GetAttributes().Any(static attribute =>
            attribute.AttributeClass?.ToDisplayString() is "Avalonia.Metadata.ContentAttribute" or "Avalonia.Metadata.ContentPropertyAttribute");

    internal static GeneratorMetadata GetMetadata(ISymbol symbol)
    {
        foreach (var attribute in symbol.GetAttributes())
        {
            if (attribute.AttributeClass?.ToDisplayString() != "CSharpUI.Avalonia.CSharpUIGeneratorMetadataAttribute")
            {
                continue;
            }

            var ignore = false;
            var generateFactory = true;
            var content = false;
            string? alias = null;
            foreach (var argument in attribute.NamedArguments)
            {
                switch (argument.Key)
                {
                    case "Ignore" when argument.Value.Value is bool ignoreValue:
                        ignore = ignoreValue;
                        break;
                    case "GenerateFactory" when argument.Value.Value is bool factoryValue:
                        generateFactory = factoryValue;
                        break;
                    case "Content" when argument.Value.Value is bool contentValue:
                        content = contentValue;
                        break;
                    case "Alias" when argument.Value.Value is string aliasValue:
                        alias = aliasValue;
                        break;
                }
            }

            return new GeneratorMetadata(ignore, generateFactory, content, alias);
        }

        return new GeneratorMetadata(false, true, false, null);
    }

    private static PropertyInheritanceKind GetInheritanceKind(bool isOverride, bool hidesBaseProperty) =>
        isOverride ? PropertyInheritanceKind.Override :
        hidesBaseProperty ? PropertyInheritanceKind.Shadow :
        PropertyInheritanceKind.Declared;

    internal static AvaloniaPropertyOwnershipKind GetOwnershipKind(IFieldSymbol field)
    {
        var initializer = field.DeclaringSyntaxReferences
            .Select(static reference => reference.GetSyntax())
            .OfType<VariableDeclaratorSyntax>()
            .Select(static variable => variable.Initializer?.Value)
            .FirstOrDefault(static value => value is not null);

        var invokedMethodNames = initializer?.DescendantNodesAndSelf()
            .OfType<InvocationExpressionSyntax>()
            .Select(static invocation => invocation.Expression switch
            {
                MemberAccessExpressionSyntax memberAccess => memberAccess.Name.Identifier.ValueText,
                IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
                _ => string.Empty
            })
            .ToArray() ?? [];

        if (invokedMethodNames.Contains("AddOwner", StringComparer.Ordinal))
        {
            return AvaloniaPropertyOwnershipKind.AddedOwner;
        }

        if (invokedMethodNames.Contains("OverrideMetadata", StringComparer.Ordinal))
        {
            return AvaloniaPropertyOwnershipKind.MetadataOverride;
        }

        return field.ContainingType is null
            ? AvaloniaPropertyOwnershipKind.Inherited
            : AvaloniaPropertyOwnershipKind.Registered;
    }
}

internal readonly struct GeneratorMetadata(bool ignore, bool generateFactory, bool content, string? alias)
{
    internal bool Ignore { get; } = ignore;

    internal bool GenerateFactory { get; } = generateFactory;

    internal bool Content { get; } = content;

    internal string? Alias { get; } = alias;
}
