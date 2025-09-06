using Avalonia.Data.Converters;
using System.Globalization;

namespace CSharpUI.Avalonia.CommonExtensions;

/// <summary>
/// Extensions for <see cref="IValueConverter"/>
/// </summary>
public static class ConverterExtensions
{
    /// <summary>
    /// Tries to convert value using specified converter
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <param name="converter"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TOut TryConvert<TIn, TOut>(this FuncValueConverter<TIn, TOut> converter, TIn value)
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
        return (TOut)converter.Convert(value, typeof(TOut), null, CultureInfo.InvariantCulture);
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }
}