#nullable enable
using Avalonia.Data;
using Avalonia.Data.Converters;
using CSharpUI.Avalonia.Styles;
using CSharpUI.Avalonia.CommonExtensions;
using Avalonia;
using System;
using System.Windows.Input;
using Tests;

namespace CSharpUI.Avalonia;

public static partial class AttachedPropertyTestExtensions
{
    //================= Attached Properties ======================//
    // Command

    /*AttachedPropertyBindFromExpressionSetterGenerator*/
    /// <summary>Accessor for Attached property .</summary>
    public static AttachedPropertyTest Command(this AttachedPropertyTest control, ICommand value)
    {
        AttachedPropertyTest.SetCommand(control, value);
        return control;
    }



}