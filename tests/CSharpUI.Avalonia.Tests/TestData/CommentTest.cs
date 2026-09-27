using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Media.TextFormatting;
using Avalonia.Metadata;
using System.Collections;
using static Avalonia.Controls.AutoCompleteBox;

namespace Tests;

public class CommentTest : TemplatedControl
{
    /// <summary>
    /// Gets or sets a collection that is used to generate the items for the
    /// drop-down portion of the <see cref="AutoCompleteBox" /> control.
    /// </summary>
    /// <value>The collection that is used to generate the items of the
    /// drop-down portion of the <see cref="AutoCompleteBox" /> control.</value>
    public IEnumerable? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly StyledProperty<IEnumerable?> ItemsSourceProperty =
        AvaloniaProperty.Register<AutoCompleteBox, IEnumerable?>(
            nameof(ItemsSource));

    /// <summary>
    /// Gets or sets the  <see cref="T:Avalonia.Data.Binding" /> that
    /// is used to get the values for display in the text portion of
    /// the <see cref="AutoCompleteBox" />
    /// control.
    /// </summary>
    /// <value>The <see cref="T:Avalonia.Data.BindingBase" /> object used
    /// when binding to a collection property.</value>
    [AssignBinding]
    [InheritDataTypeFromItems(nameof(ItemsSource))]
    public BindingBase? ValueMemberBinding
    {
        get => _valueBinding;
        set => _valueBinding = value;
    }

    private BindingBase? _valueBinding;
}
