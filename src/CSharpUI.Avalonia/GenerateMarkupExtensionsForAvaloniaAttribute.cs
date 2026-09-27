using System;

namespace CSharpUI.Avalonia;

/// <summary>
/// Indicates that markup extensions should be generated for Avalonia types in this assembly.
/// </summary>
/// <param name="generatePublicExtensions">A value indicating whether public extension members are generated.</param>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
public sealed class GenerateMarkupExtensionsForAvaloniaAttribute(bool generatePublicExtensions = false) : Attribute
{
    /// <summary>
    /// Gets a value indicating whether public extension members are generated.
    /// </summary>
    public bool GeneratePublicExtensions { get; } = generatePublicExtensions;
}
