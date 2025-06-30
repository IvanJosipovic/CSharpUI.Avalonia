#nullable enable
using Avalonia.Data;
using Avalonia.Data.Converters;
using CSharpUIAvalonia.Styles;
using CSharpUIAvalonia.CommonExtensions;
using System;
using Tests;

namespace CSharpUIAvalonia;

public static partial class GenericBaseTestExtensions
{
    //================= Common Properties ======================//
    // ViewModel

    /*ValueSetterGenerator*/
    /// <summary></summary>
    public static T ViewModel<T, TViewModel>(this T control, TViewModel value) where T : GenericBaseTest<TViewModel> where TViewModel : class
        => control._set(() => control.ViewModel = value!);



}