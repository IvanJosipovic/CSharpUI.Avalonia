using Avalonia.Data.Converters;

namespace CSharpUI.Avalonia.CommonExtensions;

/// <summary>
///Extensions for <see cref="Binding"/>
/// </summary>
public static class BindingExtensions
{
    /// <summary>
    /// Sets the specified value converter on the binding and returns the updated binding.
    /// </summary>
    /// <typeparam name="TBinding">The type of the binding, which must inherit from <see cref="Binding"/>.</typeparam>
    /// <param name="binding">The binding to which the converter will be applied. Cannot be <see langword="null"/>.</param>
    /// <param name="converter">The value converter to set on the binding. Cannot be <see langword="null"/>.</param>
    /// <returns>The binding with the specified converter applied.</returns>
    public static TBinding Converter<TBinding>(this TBinding binding, IValueConverter converter)
        where TBinding : Binding
    {
        binding.Converter = converter;
        return binding;
    }

}