using Avalonia;
using Avalonia.Controls;

namespace CSharpUI.Avalonia.Samples.CustomControls.Controls;

public sealed class LevelBadge : ContentControl
{
    public static readonly StyledProperty<BadgeLevel> LevelProperty =
        AvaloniaProperty.Register<LevelBadge, BadgeLevel>(nameof(Level));

    public static readonly AttachedProperty<bool> EmphasizedProperty =
        AvaloniaProperty.RegisterAttached<LevelBadge, Control, bool>("Emphasized");

    [CSharpUIGeneratorMetadata(Alias = "Importance")]
    public BadgeLevel Level
    {
        get => GetValue(LevelProperty);
        set => SetValue(LevelProperty, value);
    }

    public event EventHandler? Activated;

    public static bool GetEmphasized(AvaloniaObject element) => element.GetValue(EmphasizedProperty);

    public static void SetEmphasized(AvaloniaObject element, bool value) => element.SetValue(EmphasizedProperty, value);

    public void Activate() => Activated?.Invoke(this, EventArgs.Empty);
}

public enum BadgeLevel
{
    Normal,
    Important,
    Urgent
}
