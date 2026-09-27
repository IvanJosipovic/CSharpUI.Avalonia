using Microsoft.CodeAnalysis;

namespace CSharpUI.Avalonia.SourceGenerator.Analysis;

internal enum PropertyInheritanceKind
{
    Declared,
    Inherited,
    Override,
    Shadow
}

internal enum AvaloniaPropertyOwnershipKind
{
    Registered,
    Inherited,
    AddedOwner,
    MetadataOverride
}

internal sealed class ConstructorModel(IMethodSymbol symbol, bool isAccessibleFromGeneratedCode)
{
    internal IMethodSymbol Symbol { get; } = symbol;

    internal bool IsAccessibleFromGeneratedCode { get; } = isAccessibleFromGeneratedCode;

    internal bool IsParameterless => Symbol.Parameters.Length == 0;
}

internal sealed class PropertyModel(
    string name,
    string extensionName,
    INamedTypeSymbol declaredOn,
    IPropertySymbol? clrProperty,
    IFieldSymbol? avaloniaPropertyField,
    ITypeSymbol valueType,
    PropertyInheritanceKind inheritanceKind,
    AvaloniaPropertyOwnershipKind ownershipKind,
    bool isIgnored,
    bool isContentSlot)
{
    internal string Name { get; } = name;

    internal string ExtensionName { get; } = extensionName;

    internal INamedTypeSymbol DeclaredOn { get; } = declaredOn;

    internal IPropertySymbol? ClrProperty { get; } = clrProperty;

    internal IFieldSymbol? AvaloniaPropertyField { get; } = avaloniaPropertyField;

    internal ITypeSymbol ValueType { get; } = valueType;

    internal PropertyInheritanceKind InheritanceKind { get; } = inheritanceKind;

    internal AvaloniaPropertyOwnershipKind OwnershipKind { get; } = ownershipKind;

    internal bool IsIgnored { get; } = isIgnored;

    internal bool IsContentSlot { get; } = isContentSlot;

    internal bool CanRead => ClrProperty?.GetMethod?.DeclaredAccessibility == Accessibility.Public;

    internal bool CanWrite => ClrProperty?.SetMethod?.DeclaredAccessibility == Accessibility.Public;

    internal bool CanBind => AvaloniaPropertyField is not null && !AvaloniaPropertyField.HasUnsupportedExternalValueType();

    internal bool CanClear => AvaloniaPropertyField is not null;

    internal PropertyModel AsInherited() => new(
        Name,
        ExtensionName,
        DeclaredOn,
        ClrProperty,
        AvaloniaPropertyField,
        ValueType,
        PropertyInheritanceKind.Inherited,
        AvaloniaPropertyOwnershipKind.Inherited,
        IsIgnored,
        IsContentSlot);
}

internal sealed class EventModel(
    IEventSymbol symbol,
    PropertyInheritanceKind inheritanceKind,
    string extensionName,
    bool isIgnored)
{
    internal IEventSymbol Symbol { get; } = symbol;

    internal string Name => Symbol.Name;

    internal string ExtensionName { get; } = extensionName;

    internal INamedTypeSymbol DeclaredOn => Symbol.ContainingType;

    internal PropertyInheritanceKind InheritanceKind { get; } = inheritanceKind;

    internal bool IsIgnored { get; } = isIgnored;

    internal EventModel AsInherited() => new(Symbol, PropertyInheritanceKind.Inherited, ExtensionName, IsIgnored);
}

internal sealed class AttachedPropertyModel(
    IFieldSymbol attachedField,
    PropertyInheritanceKind inheritanceKind,
    AvaloniaPropertyOwnershipKind ownershipKind,
    string extensionName,
    bool isIgnored)
{
    internal IFieldSymbol Field { get; } = attachedField;

    internal string Name => Field.GetMemberName();

    internal string ExtensionName { get; } = extensionName;

    internal INamedTypeSymbol DeclaredOn => Field.ContainingType!;

    internal PropertyInheritanceKind InheritanceKind { get; } = inheritanceKind;

    internal AvaloniaPropertyOwnershipKind OwnershipKind { get; } = ownershipKind;

    internal bool IsIgnored { get; } = isIgnored;

    internal AttachedPropertyModel AsInherited() => new(
        Field,
        PropertyInheritanceKind.Inherited,
        AvaloniaPropertyOwnershipKind.Inherited,
        ExtensionName,
        IsIgnored);
}

internal sealed class ContentSlotModel(IPropertySymbol property)
{
    internal IPropertySymbol Property { get; } = property;

    internal string Name => Property.Name;

    internal ITypeSymbol ValueType => Property.Type;
}

internal sealed class ControlModel(
    INamedTypeSymbol type,
    ControlModel? baseType,
    IReadOnlyList<ConstructorModel> constructors,
    IReadOnlyList<PropertyModel> declaredProperties,
    IReadOnlyList<PropertyModel> properties,
    IReadOnlyList<EventModel> declaredEvents,
    IReadOnlyList<EventModel> events,
    IReadOnlyList<AttachedPropertyModel> declaredAttachedProperties,
    IReadOnlyList<AttachedPropertyModel> attachedProperties,
    IReadOnlyList<IFieldSymbol> stylePropertyFields,
    IReadOnlyList<ContentSlotModel> contentSlots,
    bool ignoreGeneratedExtensions,
    bool generateFactory,
    string factoryName)
{
    internal INamedTypeSymbol Type { get; } = type;

    internal ControlModel? BaseType { get; } = baseType;

    internal bool IsAbstract => Type.IsAbstract;

    internal bool IsSealed => Type.IsSealed;

    internal bool IsGeneric => Type.IsGenericType;

    internal IReadOnlyList<ConstructorModel> Constructors { get; } = constructors;

    internal IReadOnlyList<PropertyModel> DeclaredProperties { get; } = declaredProperties;

    internal IReadOnlyList<PropertyModel> Properties { get; } = properties;

    internal IReadOnlyList<EventModel> DeclaredEvents { get; } = declaredEvents;

    internal IReadOnlyList<EventModel> Events { get; } = events;

    internal IReadOnlyList<AttachedPropertyModel> DeclaredAttachedProperties { get; } = declaredAttachedProperties;

    internal IReadOnlyList<AttachedPropertyModel> AttachedProperties { get; } = attachedProperties;

    internal IReadOnlyList<IFieldSymbol> StylePropertyFields { get; } = stylePropertyFields;

    internal IReadOnlyList<ContentSlotModel> ContentSlots { get; } = contentSlots;

    internal bool IgnoreGeneratedExtensions { get; } = ignoreGeneratedExtensions;

    internal bool GenerateFactory { get; } = generateFactory;

    internal string FactoryName { get; } = factoryName;

    internal bool IsAvaloniaObject => Type.IsOrInheritsFrom("Avalonia.AvaloniaObject");

    internal bool IsConstructible =>
        !IsAbstract && !IsGeneric && Constructors.Any(static constructor =>
            constructor.IsAccessibleFromGeneratedCode && constructor.IsParameterless);
}
