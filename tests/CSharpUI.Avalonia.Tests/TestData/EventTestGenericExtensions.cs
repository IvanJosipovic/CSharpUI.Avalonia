#nullable enable
using Avalonia.Data;
using CSharpUI.Avalonia.CommonExtensions;
using CSharpUI.Avalonia.Styles;
using System;
using System.ComponentModel;
using Tests;

namespace CSharpUI.Avalonia.Extensions;

public static partial class EventTestGenericExtensions
{
    //================= Events ======================//
    // DropDownClosing

    /*ActionToEventGenerator*/
    public static T OnDropDownClosing<T>(this T control, Action<object?, CancelEventArgs> action) where T : EventTestGeneric
        => control._setEvent((EventHandler<CancelEventArgs>)((arg0, arg1) => action(arg0, arg1)), h => control.DropDownClosing += h);



}