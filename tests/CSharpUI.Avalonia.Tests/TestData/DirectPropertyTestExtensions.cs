#nullable enable

namespace CSharpUI.Avalonia.Extensions;

public static partial class DirectPropertyTestExtensions
{
    //================= Properties ======================//
    // CanSave

    /*ValueSetterGenerator*/
    /// <summary>My Comment</summary>
    public static T CanSave<T>(this T control, global::System.Boolean value) where T : global::Tests.DirectPropertyTest
        => control._set(() => control.CanSave = value!);

    /*AvaloniaPropertyBindSetterGenerator*/
    /// <summary>My Comment</summary>
    public static T CanSave<T>(this T control, global::Avalonia.AvaloniaProperty avaloniaProperty, global::Avalonia.Data.BindingMode? bindingMode = null, global::Avalonia.Data.Converters.IValueConverter? converter = null) where T : global::Tests.DirectPropertyTest
        => control._set(global::Tests.DirectPropertyTest.CanSaveProperty, avaloniaProperty, bindingMode, converter);



}