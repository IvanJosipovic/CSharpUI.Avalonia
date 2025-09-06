#nullable enable

namespace CSharpUI.Avalonia.Extensions;

public static partial class CommonPropertyTestExtensions
{
    //================= Common Properties ======================//
    // MyString

    /*ValueSetterGenerator*/
    /// <summary>My Comment</summary>
    public static T MyString<T>(this T control, global::System.String value) where T : global::Tests.CommonPropertyTest
        => control._set(() => control.MyString = value!);



}