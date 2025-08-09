#nullable enable
using Avalonia.Data;
using Avalonia.Data.Converters;
using CSharpUIAvalonia.Styles;
using CSharpUIAvalonia.CommonExtensions;
using System;
using System.ComponentModel;
using Tests;

namespace CSharpUIAvalonia;

public static partial class EventTestGenericExtensions
{
    //================= Events ======================//
    // DropDownClosing

    /*ActionToEventGenerator*/
    public static T OnDropDownClosing<T>(this T control, Action<object?, CancelEventArgs> action) where T : EventTestGeneric
        => control._setEvent((EventHandler<CancelEventArgs>)((arg0, arg1) => action(arg0, arg1)), h => control.DropDownClosing += h);



}