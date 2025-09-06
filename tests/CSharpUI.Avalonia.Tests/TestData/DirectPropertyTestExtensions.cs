#nullable enable

namespace CSharpUI.Avalonia.Extensions;

public static partial class DirectPropertyTestExtensions
{
    //================= Properties ======================//
    // CanSave

    /*ValueSetterGenerator*/
    /// <summary>My Comment</summary>
    public static T CanSave<T>(this T control, bool value) where T : global::Tests.DirectPropertyTest
        => control._set(() => control.CanSave = value!);



}