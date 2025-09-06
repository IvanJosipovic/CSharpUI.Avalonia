using Avalonia.Styling;
using CSharpUI.Avalonia.Styles;

namespace CSharpUI.Avalonia.CommonExtensions;

/// <summary>
/// Extensions for Style and Style&lt;TElement&gt;
/// </summary>
public static class StylePropertyExtensions
{
    /// <summary>
    /// Creates selector and applies .OfType&lt;TElement&gt;() to it
    /// </summary>
    /// <typeparam name="TElement">Type of the control that style will be applied</typeparam>
    /// <param name="style">Style</param>
    /// <param name="selector">Selector modifier function</param>
    /// <returns>style with applied selector</returns>
    public static Style<TElement> Selector<TElement>(this Style<TElement> style, Func<Selector, Selector> selector)
        where TElement : StyledElement
    {
        Selector TypeSelector(Selector? s) => s.OfType<TElement>();
        style.Selector = selector(TypeSelector(null));
        return style;
    }

    /// <summary>
    /// Creates selector and applies selector function to it
    /// </summary>
    /// <param name="style"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static Style Selector(this Style style, Func<Selector?, Selector> selector)
    {
        style.Selector = selector(null);
        return style;
    }

    /// <summary>
    /// Adds Setter to Style and returns the Style for method chaining
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="style"></param>
    /// <param name="avaloniaProperty"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Style<TElement> Setter<TElement>(this Style<TElement> style, AvaloniaProperty avaloniaProperty, object value)
        where TElement : StyledElement
    {
        style._addSetter(avaloniaProperty, value);
        return style;
    }

    /// <summary>
    /// Adds Setter to Style and returns the Style for method chaining
    /// </summary>
    /// <param name="style"></param>
    /// <param name="avaloniaProperty"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Style Setter(this Style style, AvaloniaProperty avaloniaProperty, object value)
    {
        style.Setters.Add(new Setter(avaloniaProperty, value));
        return style;
    }

    /// <summary>
    /// Adds Setter to Style and returns the Style for method chaining
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="style"></param>
    /// <param name="avaloniaProperty"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Style<TElement> _addSetter<TElement>(this Style<TElement> style, AvaloniaProperty avaloniaProperty, object value)
        where TElement : StyledElement
    {
        style.Setters.Add(new Setter(avaloniaProperty, value));
        return style;
    }

    /// <summary>
    /// Adds Grid.Column setter to Style and returns the Style for method chaining
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="style"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Style<TElement> Col<TElement>(this Style<TElement> style, int value)
        where TElement : Control
    {
        style.Add(new Setter(Grid.ColumnProperty, value));
        return style;
    }

    /// <summary>
    /// Adds Grid.Row setter to Style and returns the Style for method chaining
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="style"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Style<TElement> Row<TElement>(this Style<TElement> style, int value)
        where TElement : Control
    {
        style.Add(new Setter(Grid.RowProperty, value));
        return style;
    }

    /// <summary>
    /// Adds Grid.ColumnSpan setter to Style and returns the Style for method chaining
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="style"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Style<TElement> ColSpan<TElement>(this Style<TElement> style, int value)
        where TElement : Control
    {
        style.Add(new Setter(Grid.ColumnSpanProperty, value));
        return style;
    }

    /// <summary>
    /// Adds Grid.RowSpan setter to Style and returns the Style for method chaining
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="style"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Style<TElement> RowSpan<TElement>(this Style<TElement> style, int value)
        where TElement : Control
    {
        style.Add(new Setter(Grid.RowSpanProperty, value));
        return style;
    }
}