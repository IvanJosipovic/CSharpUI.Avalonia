using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Metadata;

namespace Tests
{
    public class AttachedProperty3Test : TemplatedControl
    {
        [Unstable("To be removed in 12.0, NativeMenuBar now has a default template")] // TODO12
        public static readonly AttachedProperty<bool> EnableMenuItemClickForwardingProperty =
            AvaloniaProperty.RegisterAttached<NativeMenuBar, MenuItem, bool>(
                "EnableMenuItemClickForwarding");


        [Unstable("To be removed in 12.0, NativeMenuBar now has a default template.")] // TODO12
        public static void SetEnableMenuItemClickForwarding(MenuItem menuItem, bool enable)
        {
            menuItem.SetValue(EnableMenuItemClickForwardingProperty, enable);
        }
    }
}