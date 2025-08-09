#nullable enable
using Avalonia.Data;
using Avalonia.Data.Converters;
using CSharpUIAvalonia.Styles;
using CSharpUIAvalonia.CommonExtensions;
using System;
using Tests;

namespace CSharpUIAvalonia;

public static partial class EventTestExtensions
{
    //================= Events ======================//
    // DropDownClosed

    /*ActionToEventGenerator*/
    public static T OnDropDownClosed<T>(this T control, Action<object?, EventArgs> action) where T : EventTest
        => control._setEvent((EventHandler)((arg0, arg1) => action(arg0, arg1)), h => control.DropDownClosed += h);

}