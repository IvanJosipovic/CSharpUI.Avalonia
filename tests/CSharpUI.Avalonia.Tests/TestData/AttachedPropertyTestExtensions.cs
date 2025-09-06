#nullable enable
using Avalonia.Data;
using Avalonia.Data.Converters;
using CSharpUI.Avalonia.Styles;
using CSharpUI.Avalonia.CommonExtensions;
using Avalonia;
using System;
using System.Windows.Input;
using Tests;

namespace CSharpUI.Avalonia.Extensions;

public static partial class AttachedPropertyTestExtensions
{
    //================= Attached Properties ======================//
    // Command

    /*AttachedPropertyBindFromExpressionSetterGenerator*/
    /// <summary>Accessor for Attached property .</summary>
    public static T Command<T>(this T control, global::System.Windows.Input.ICommand value) where T : global::Tests.AttachedPropertyTest
    {
        global::Tests.AttachedPropertyTest.SetCommand(control, value);
        return control;
    }



}