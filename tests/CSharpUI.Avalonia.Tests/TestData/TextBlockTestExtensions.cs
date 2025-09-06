#nullable enable
using Avalonia.Data;
using CSharpUI.Avalonia.Styles;
using CSharpUI.Avalonia.CommonExtensions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using System;
using System.Collections.Generic;
using Tests;

namespace CSharpUI.Avalonia.Extensions;

public static partial class TextBlockTestExtensions
{
    //================= Properties ======================//
    // Background

    /*ValueSetterGenerator*/
    /// <summary>Gets or sets a brush used to paint the control's background.</summary>
    public static T Background<T>(this T control, global::Avalonia.Media.IBrush? value) where T : global::Tests.TextBlockTest
        => control._set(() => control.Background = value!);


    // Padding

    /*ValueSetterGenerator*/
    /// <summary>Gets or sets the padding to place around the .</summary>
    public static T Padding<T>(this T control, global::Avalonia.Thickness value) where T : global::Tests.TextBlockTest
        => control._set(() => control.Padding = value!);

    /*ValueOverloadsSetterGenerator*/

    public static T Padding<T>(this T control, double uniformLength = default) where T : global::Tests.TextBlockTest
        => control._set(() => control.Padding = new global::Avalonia.Thickness(uniformLength));
    public static T Padding<T>(this T control, double horizontal = default, double vertical = default) where T : global::Tests.TextBlockTest
        => control._set(() => control.Padding = new global::Avalonia.Thickness(horizontal, vertical));
    public static T Padding<T>(this T control, double left = default, double top = default, double right = default, double bottom = default) where T : global::Tests.TextBlockTest
        => control._set(() => control.Padding = new global::Avalonia.Thickness(left, top, right, bottom));


    // FontFamily

    /*ValueSetterGenerator*/
    /// <summary>Gets or sets the font family used to draw the control's text.</summary>
    public static T FontFamily<T>(this T control, global::Avalonia.Media.FontFamily value) where T : global::Tests.TextBlockTest
        => control._set(() => control.FontFamily = value!);


    // FontSize

    /*ValueSetterGenerator*/
    /// <summary>Gets or sets the size of the control's text in points.</summary>
    public static T FontSize<T>(this T control, double value) where T : global::Tests.TextBlockTest
        => control._set(() => control.FontSize = value!);


    // FontStyle

    /*ValueSetterGenerator*/
    /// <summary>Gets or sets the font style used to draw the control's text.</summary>
    public static T FontStyle<T>(this T control, global::Avalonia.Media.FontStyle value) where T : global::Tests.TextBlockTest
        => control._set(() => control.FontStyle = value!);


    // FontWeight

    /*ValueSetterGenerator*/
    /// <summary>Gets or sets the font weight used to draw the control's text.</summary>
    public static T FontWeight<T>(this T control, global::Avalonia.Media.FontWeight value) where T : global::Tests.TextBlockTest
        => control._set(() => control.FontWeight = value!);


    // FontStretch

    /*ValueSetterGenerator*/
    /// <summary>Gets or sets the font stretch used to draw the control's text.</summary>
    public static T FontStretch<T>(this T control, global::Avalonia.Media.FontStretch value) where T : global::Tests.TextBlockTest
        => control._set(() => control.FontStretch = value!);


    // Foreground

    /*ValueSetterGenerator*/
    /// <summary>Gets or sets the brush used to draw the control's text and other foreground elements.</summary>
    public static T Foreground<T>(this T control, global::Avalonia.Media.IBrush? value) where T : global::Tests.TextBlockTest
        => control._set(() => control.Foreground = value!);


    // Text

    /*ValueSetterGenerator*/
    /// <summary>Gets or sets the text.</summary>
    public static T Text<T>(this T control, string? value) where T : global::Tests.TextBlockTest
        => control._set(() => control.Text = value!);


    // TextDecorations

    /*ValueSetterGenerator*/
    /// <summary>Gets or sets the text decorations.</summary>
    public static T TextDecorations<T>(this T control, global::Avalonia.Media.TextDecorationCollection? value) where T : global::Tests.TextBlockTest
        => control._set(() => control.TextDecorations = value!);


    // FontFeatures

    /*ValueSetterGenerator*/
    /// <summary>Gets or sets the font features.</summary>
    public static T FontFeatures<T>(this T control, global::Avalonia.Media.FontFeatureCollection? value) where T : global::Tests.TextBlockTest
        => control._set(() => control.FontFeatures = value!);


    // Inlines

    /*ValueSetterGenerator*/
    /// <summary>Gets or sets the inlines.</summary>
    public static T Inlines<T>(this T control, global::Avalonia.Controls.Documents.InlineCollection? value) where T : global::Tests.TextBlockTest
        => control._set(() => control.Inlines = value!);



    //================= Attached Properties ======================//
    // BaselineOffset

    /*AttachedPropertyBindFromExpressionSetterGenerator*/
    /// <summary>Writes the attached property BaselineOffset to the given element.</summary>
    public static T BaselineOffset<T>(this T control, double value) where T : global::Tests.TextBlockTest
    {
        global::Tests.TextBlockTest.SetBaselineOffset(control, value);
        return control;
    }


    // LineHeight

    /*AttachedPropertyBindFromExpressionSetterGenerator*/
    /// <summary>Writes the attached property BaselineOffset to the given element.</summary>
    public static T LineHeight<T>(this T control, double value) where T : global::Tests.TextBlockTest
    {
        global::Tests.TextBlockTest.SetLineHeight(control, value);
        return control;
    }


    // LineSpacing

    /*AttachedPropertyBindFromExpressionSetterGenerator*/
    /// <summary></summary>
    public static T LineSpacing<T>(this T control, double value) where T : global::Tests.TextBlockTest
    {
        //global::Tests.TextBlockTest.SetLineSpacing(control, value);
        return control;
    }


    // LetterSpacing

    /*AttachedPropertyBindFromExpressionSetterGenerator*/
    /// <summary>Writes the attached property LetterSpacing to the given element.</summary>
    public static T LetterSpacing<T>(this T control, double value) where T : global::Tests.TextBlockTest
    {
        global::Tests.TextBlockTest.SetLetterSpacing(control, value);
        return control;
    }


    // MaxLines

    /*AttachedPropertyBindFromExpressionSetterGenerator*/
    /// <summary>Writes the attached property BaselineOffset to the given element.</summary>
    public static T MaxLines<T>(this T control, int value) where T : global::Tests.TextBlockTest
    {
        global::Tests.TextBlockTest.SetMaxLines(control, value);
        return control;
    }


    // TextAlignment

    /*AttachedPropertyBindFromExpressionSetterGenerator*/
    /// <summary>Writes the attached property BaselineOffset to the given element.</summary>
    public static T TextAlignment<T>(this T control, global::Avalonia.Media.TextAlignment value) where T : global::Tests.TextBlockTest
    {
        global::Tests.TextBlockTest.SetTextAlignment(control, value);
        return control;
    }


    // TextWrapping

    /*AttachedPropertyBindFromExpressionSetterGenerator*/
    /// <summary>Writes the attached property BaselineOffset to the given element.</summary>
    public static T TextWrapping<T>(this T control, global::Avalonia.Media.TextWrapping value) where T : global::Tests.TextBlockTest
    {
        global::Tests.TextBlockTest.SetTextWrapping(control, value);
        return control;
    }


    // TextTrimming

    /*AttachedPropertyBindFromExpressionSetterGenerator*/
    /// <summary>Writes the attached property BaselineOffset to the given element.</summary>
    public static T TextTrimming<T>(this T control, global::Avalonia.Media.TextTrimming value) where T : global::Tests.TextBlockTest
    {
        global::Tests.TextBlockTest.SetTextTrimming(control, value);
        return control;
    }



    //================= Styles ======================//
    // Background

    /*ValueStyleSetterGenerator*/
    public static Style<T> Background<T>(this Style<T> style, global::Avalonia.Media.IBrush? value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.BackgroundProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> Background<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.BackgroundProperty, binding);


    // Padding

    /*ValueStyleSetterGenerator*/
    public static Style<T> Padding<T>(this Style<T> style, global::Avalonia.Thickness value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.PaddingProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> Padding<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.PaddingProperty, binding);

    /*ValueOverloadsStyleSetterGenerator*/
    public static Style<T> Padding<T>(this Style<T> style, double uniformLength) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.PaddingProperty, new global::Avalonia.Thickness(uniformLength));
    public static Style<T> Padding<T>(this Style<T> style, double horizontal, double vertical) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.PaddingProperty, new global::Avalonia.Thickness(horizontal, vertical));
    public static Style<T> Padding<T>(this Style<T> style, double left, double top, double right, double bottom) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.PaddingProperty, new global::Avalonia.Thickness(left, top, right, bottom));



    // FontFamily

    /*ValueStyleSetterGenerator*/
    public static Style<T> FontFamily<T>(this Style<T> style, global::Avalonia.Media.FontFamily value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.FontFamilyProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> FontFamily<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.FontFamilyProperty, binding);


    // FontSize

    /*ValueStyleSetterGenerator*/
    public static Style<T> FontSize<T>(this Style<T> style, double value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.FontSizeProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> FontSize<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.FontSizeProperty, binding);


    // FontStyle

    /*ValueStyleSetterGenerator*/
    public static Style<T> FontStyle<T>(this Style<T> style, global::Avalonia.Media.FontStyle value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.FontStyleProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> FontStyle<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.FontStyleProperty, binding);


    // FontWeight

    /*ValueStyleSetterGenerator*/
    public static Style<T> FontWeight<T>(this Style<T> style, global::Avalonia.Media.FontWeight value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.FontWeightProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> FontWeight<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.FontWeightProperty, binding);


    // FontStretch

    /*ValueStyleSetterGenerator*/
    public static Style<T> FontStretch<T>(this Style<T> style, global::Avalonia.Media.FontStretch value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.FontStretchProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> FontStretch<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.FontStretchProperty, binding);


    // Foreground

    /*ValueStyleSetterGenerator*/
    public static Style<T> Foreground<T>(this Style<T> style, global::Avalonia.Media.IBrush? value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.ForegroundProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> Foreground<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.ForegroundProperty, binding);


    // BaselineOffset

    /*ValueStyleSetterGenerator*/
    public static Style<T> BaselineOffset<T>(this Style<T> style, double value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.BaselineOffsetProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> BaselineOffset<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.BaselineOffsetProperty, binding);


    // LineHeight

    /*ValueStyleSetterGenerator*/
    public static Style<T> LineHeight<T>(this Style<T> style, double value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.LineHeightProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> LineHeight<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.LineHeightProperty, binding);


    // LineSpacing

    /*ValueStyleSetterGenerator*/
    public static Style<T> LineSpacing<T>(this Style<T> style, double value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.LineSpacingProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> LineSpacing<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.LineSpacingProperty, binding);


    // LetterSpacing

    /*ValueStyleSetterGenerator*/
    public static Style<T> LetterSpacing<T>(this Style<T> style, double value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.LetterSpacingProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> LetterSpacing<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.LetterSpacingProperty, binding);


    // MaxLines

    /*ValueStyleSetterGenerator*/
    public static Style<T> MaxLines<T>(this Style<T> style, int value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.MaxLinesProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> MaxLines<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.MaxLinesProperty, binding);


    // Text

    /*ValueStyleSetterGenerator*/
    public static Style<T> Text<T>(this Style<T> style, string? value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.TextProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> Text<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.TextProperty, binding);


    // TextAlignment

    /*ValueStyleSetterGenerator*/
    public static Style<T> TextAlignment<T>(this Style<T> style, global::Avalonia.Media.TextAlignment value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.TextAlignmentProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> TextAlignment<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.TextAlignmentProperty, binding);


    // TextWrapping

    /*ValueStyleSetterGenerator*/
    public static Style<T> TextWrapping<T>(this Style<T> style, global::Avalonia.Media.TextWrapping value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.TextWrappingProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> TextWrapping<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.TextWrappingProperty, binding);


    // TextTrimming

    /*ValueStyleSetterGenerator*/
    public static Style<T> TextTrimming<T>(this Style<T> style, global::Avalonia.Media.TextTrimming value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.TextTrimmingProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> TextTrimming<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.TextTrimmingProperty, binding);


    // TextDecorations

    /*ValueStyleSetterGenerator*/
    public static Style<T> TextDecorations<T>(this Style<T> style, global::Avalonia.Media.TextDecorationCollection? value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.TextDecorationsProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> TextDecorations<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.TextDecorationsProperty, binding);


    // FontFeatures

    /*ValueStyleSetterGenerator*/
    public static Style<T> FontFeatures<T>(this Style<T> style, global::Avalonia.Media.FontFeatureCollection? value) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.FontFeaturesProperty!, value!);

    /*BindingStyleSetterGenerator*/
    public static Style<T> FontFeatures<T>(this Style<T> style, IBinding binding) where T : global::Tests.TextBlockTest
        => style._addSetter(global::Tests.TextBlockTest.FontFeaturesProperty, binding);



}