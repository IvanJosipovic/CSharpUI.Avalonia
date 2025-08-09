using Avalonia.Styling;

namespace CSharpUI.Avalonia.Styles;

public class StyleGroup(Func<Selector,Selector>? groupSelectorFunc = null) : List<object>
{
    public Func<Selector, Selector>? GroupSelectorFunc { get; } = groupSelectorFunc;

    public override string ToString()
    {
        if (GroupSelectorFunc != null)
        {
            return GroupSelectorFunc(null!).ToString();
        }
        return base.ToString() ?? "- No selector --";
    }
}