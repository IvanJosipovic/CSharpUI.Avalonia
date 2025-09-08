#nullable enable

namespace CSharpUI.Avalonia.Extensions;

public static partial class StyledPropertyTestExtensions
{
    //================= Properties ======================//
    // CanSave

    /*ValueSetterGenerator*/
    /// <summary>My Comment</summary>
    public static T CanSave<T>(this T control, global::System.Boolean value) where T : global::Tests.StyledPropertyTest
        => control._set(() => control.CanSave = value!);

    /*AvaloniaPropertyBindSetterGenerator*/
    /// <summary>My Comment</summary>
    public static T CanSave<T>(this T control, global::Avalonia.AvaloniaProperty avaloniaProperty, global::Avalonia.Data.BindingMode? bindingMode = null, global::Avalonia.Data.Converters.IValueConverter? converter = null) where T : global::Tests.StyledPropertyTest
        => control._set(global::Tests.StyledPropertyTest.CanSaveProperty, avaloniaProperty, bindingMode, converter);



    //================= Styles ======================//
    // CanSave

    /*ValueStyleSetterGenerator*/
    /// <summary>My Comment</summary>
    public static global::CSharpUI.Avalonia.Styles.Style<T> CanSave<T>(this global::CSharpUI.Avalonia.Styles.Style<T> style, global::System.Boolean value) where T : global::Tests.StyledPropertyTest
        => style._addSetter(global::Tests.StyledPropertyTest.CanSaveProperty!, value!);

    /*BindingStyleSetterGenerator*/
    /// <summary>My Comment</summary>
    public static global::CSharpUI.Avalonia.Styles.Style<T> CanSave<T>(this global::CSharpUI.Avalonia.Styles.Style<T> style, global::Avalonia.Data.IBinding binding) where T : global::Tests.StyledPropertyTest
        => style._addSetter(global::Tests.StyledPropertyTest.CanSaveProperty, binding);



}