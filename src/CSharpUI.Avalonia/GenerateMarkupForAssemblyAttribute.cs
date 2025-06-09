using System;

namespace CSharpUI.Avalonia;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class GenerateMarkupForAssemblyAttribute : Attribute
{
    public GenerateMarkupForAssemblyAttribute(Type referenceType)
    {
    }
}
