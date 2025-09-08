#nullable enable

namespace CSharpUI.Avalonia.Extensions;

public static partial class EventTestGenericExtensions
{
    //================= Events ======================//
    // DropDownClosing

    /*ActionToEventGenerator*/
    /// <summary></summary>
    public static T OnDropDownClosing<T>(this T control, global::System.Action<global::System.Object?, global::System.ComponentModel.CancelEventArgs> action) where T : global::Tests.EventTestGeneric
        => control._setEvent((global::System.EventHandler<global::System.ComponentModel.CancelEventArgs>)((arg0, arg1) => action(arg0, arg1)), h => control.DropDownClosing += h);


    // DropDownClosing2

    /*ActionToEventGenerator*/
    /// <summary></summary>
    public static T OnDropDownClosing2<T>(this T control, global::System.Action<global::System.Object?, global::System.ComponentModel.CancelEventArgs> action) where T : global::Tests.EventTestGeneric
        => control._setEvent((global::System.EventHandler<global::System.ComponentModel.CancelEventArgs>?)((arg0, arg1) => action(arg0, arg1)), h => control.DropDownClosing2 += h);



}