#nullable enable

namespace CSharpUI.Avalonia.Extensions;

public static partial class EventTestExtensions
{
    //================= Events ======================//
    // DropDownClosed

    /*ActionToEventGenerator*/
    public static T OnDropDownClosed<T>(this T control, global::System.Action<global::System.Object?, global::System.EventArgs> action) where T : global::Tests.EventTest
        => control._setEvent((global::System.EventHandler)((arg0, arg1) => action(arg0, arg1)), h => control.DropDownClosed += h);


    // DropDownClosed2

    /*ActionToEventGenerator*/
    public static T OnDropDownClosed2<T>(this T control, global::System.Action<global::System.Object?, global::System.EventArgs> action) where T : global::Tests.EventTest
        => control._setEvent((global::System.EventHandler?)((arg0, arg1) => action(arg0, arg1)), h => control.DropDownClosed2 += h);



}