using System;

namespace CSharpUI.Avalonia;

/// <summary>
/// Indicates that markup extensions should be generated for types in this assembly.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class GenerateMarkupExtensionsForAssemblyAttribute : Attribute
{
    /// <summary>
    /// Initializes the attribute with internal generated extensions.
    /// </summary>
    /// <param name="anchorType">The type used as the anchor for generating markup extensions.</param>
    public GenerateMarkupExtensionsForAssemblyAttribute(Type anchorType)
        : this(anchorType, generatePublicExtensions: false)
    {
    }

    /// <summary>
    /// Initializes the attribute with the requested generated extension accessibility.
    /// </summary>
    /// <param name="anchorType">The type used as the anchor for generating markup extensions.</param>
    /// <param name="generatePublicExtensions">A value indicating whether generated extension members are declared as public.</param>
    public GenerateMarkupExtensionsForAssemblyAttribute(Type anchorType, bool generatePublicExtensions)
    {
        AnchorType = anchorType;
        GeneratePublicExtensions = generatePublicExtensions;
    }

    /// <summary>
    /// Gets the type used as the anchor.
    /// </summary>
    public Type AnchorType { get; }

    /// <summary>
    /// Gets a value indicating whether generated extension members are declared as public.
    /// </summary>
    public bool GeneratePublicExtensions { get; }
}
