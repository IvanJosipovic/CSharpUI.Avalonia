#nullable enable
using Avalonia.Data;
using CSharpUI.Avalonia.Styles;
using CSharpUI.Avalonia.CommonExtensions;
using System;
using Tests;

namespace CSharpUI.Avalonia.Extensions;

public static partial class GenericBaseTestExtensions
{
    //================= Common Properties ======================//
    // ViewModel

    /*ValueSetterGenerator*/
    /// <summary></summary>
    public static T ViewModel<T, TViewModel>(this T control, TViewModel? value) where T : global::Tests.GenericBaseTest<TViewModel> where TViewModel : class
        => control._set(() => control.ViewModel = value!);



}
