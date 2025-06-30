namespace CSharpUIAvalonia;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class GenerateExtensionsForAssemblyAttribute : Attribute
{
    public GenerateExtensionsForAssemblyAttribute(Type referenceType)
    {
    }
}
