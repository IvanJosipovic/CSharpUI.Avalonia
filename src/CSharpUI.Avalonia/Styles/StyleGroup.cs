using Avalonia.Styling;

namespace CSharpUI.Avalonia.Styles;

/// <summary>
/// Group of styles with optional group selector
/// </summary>
/// <param name="groupSelectorFunc"></param>
public class StyleGroup(Func<Selector, Selector>? groupSelectorFunc = null) : List<object>
{
    /// <summary>
    /// Function that takes a selector and returns a modified selector for the group
    /// </summary>
    public Func<Selector, Selector>? GroupSelectorFunc { get; } = groupSelectorFunc;

    /// <summary>
    /// Uses the GroupSelectorFunc to generate a string representation of the selector
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (GroupSelectorFunc != null)
        {
            return GroupSelectorFunc(null!).ToString();
        }
        return base.ToString() ?? "- No selector --";
    }
}