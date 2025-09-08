#nullable enable

namespace CSharpUI.Avalonia.Extensions;

public static partial class CommentTestExtensions
{
    //================= Properties ======================//
    // ItemsSource

    /*ValueSetterGenerator*/
    /// <summary>Gets or sets a collection that is used to generate the items for the drop-down portion of the control.</summary>
    public static T ItemsSource<T>(this T control, global::System.Collections.IEnumerable? value) where T : global::Tests.CommentTest
        => control._set(() => control.ItemsSource = value!);

    /*AvaloniaPropertyBindSetterGenerator*/
    /// <summary>Gets or sets a collection that is used to generate the items for the drop-down portion of the control.</summary>
    public static T ItemsSource<T>(this T control, global::Avalonia.AvaloniaProperty avaloniaProperty, global::Avalonia.Data.BindingMode? bindingMode = null, global::Avalonia.Data.Converters.IValueConverter? converter = null) where T : global::Tests.CommentTest
        => control._set(global::Tests.CommentTest.ItemsSourceProperty, avaloniaProperty, bindingMode, converter);



    //================= Common Properties ======================//
    // ValueMemberBinding

    /*ValueSetterGenerator*/
    /// <summary>Gets or sets the that is used to get the values for display in the text portion of the control.</summary>
    public static T ValueMemberBinding<T>(this T control, global::Avalonia.Data.IBinding? value) where T : global::Tests.CommentTest
        => control._set(() => control.ValueMemberBinding = value!);



    //================= Styles ======================//
    // ItemsSource

    /*ValueStyleSetterGenerator*/
    /// <summary>Gets or sets a collection that is used to generate the items for the drop-down portion of the control.</summary>
    public static global::CSharpUI.Avalonia.Styles.Style<T> ItemsSource<T>(this global::CSharpUI.Avalonia.Styles.Style<T> style, global::System.Collections.IEnumerable? value) where T : global::Tests.CommentTest
        => style._addSetter(global::Tests.CommentTest.ItemsSourceProperty!, value!);

    /*BindingStyleSetterGenerator*/
    /// <summary>Gets or sets a collection that is used to generate the items for the drop-down portion of the control.</summary>
    public static global::CSharpUI.Avalonia.Styles.Style<T> ItemsSource<T>(this global::CSharpUI.Avalonia.Styles.Style<T> style, global::Avalonia.Data.IBinding binding) where T : global::Tests.CommentTest
        => style._addSetter(global::Tests.CommentTest.ItemsSourceProperty, binding);



}