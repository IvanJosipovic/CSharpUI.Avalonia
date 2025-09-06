namespace CSharpUI.Avalonia;

/// <summary>
/// Attribute to trigger source generator to create extension methods for all controls in the assembly of the specified type
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class GenerateExtensionsForAssemblyAttribute : Attribute
{
    /// <summary>
    /// Creates attribute to trigger source generator to create extension methods for all controls in the assembly of the specified type
    /// </summary>
    /// <param name="referenceType"></param>
    public GenerateExtensionsForAssemblyAttribute(Type referenceType)
    {
    }
}