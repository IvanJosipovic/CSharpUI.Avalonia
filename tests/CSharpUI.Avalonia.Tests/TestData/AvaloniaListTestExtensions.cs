#nullable enable

namespace CSharpUI.Avalonia.Extensions;

public static partial class AvaloniaListTestExtensions
{
    //================= Events ======================//
    // CollectionChanged

    /*ActionToEventGenerator*/
    /// <summary></summary>
    public static T OnCollectionChanged<T>(this T control, global::System.Action<global::System.Object?, global::System.EventArgs> action) where T : global::Tests.AvaloniaListTest
        => control._setEvent((global::System.Collections.Specialized.NotifyCollectionChangedEventHandler?)((arg0, arg1) => action(arg0, arg1)), h => control.CollectionChanged += h);



}