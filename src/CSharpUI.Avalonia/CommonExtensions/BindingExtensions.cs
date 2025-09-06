using Avalonia.Data.Converters;

namespace CSharpUI.Avalonia.CommonExtensions;

public static class BindingExtensions
{
    public static TBinding Converter<TBinding>(this TBinding binding, IValueConverter converter)
        where TBinding : Binding
    {
        binding.Converter = converter;
        return binding;
    }

}