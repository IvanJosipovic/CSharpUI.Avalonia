#nullable enable

namespace CSharpUI.Avalonia.Extensions;

public static partial class SealedClassTestExtensions
{
    //================= Properties ======================//
    // CanSave

    /*ValueSetterGenerator*/
    /// <summary>My Comment</summary>
    public static global::Tests.SealedClassTest CanSave(this global::Tests.SealedClassTest control, global::System.Boolean value)
        => control._set(() => control.CanSave = value!);



}