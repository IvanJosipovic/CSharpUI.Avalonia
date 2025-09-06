namespace CSharpUI.Avalonia.CommonExtensions;

/// <summary>
/// Extensions for NativeMenu and NativeMenuItem
/// </summary>
public static class NativeMenuExtensions
{
    /// <summary>
    /// Adds items to NativeMenu and returns the menu for method chaining
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    public static NativeMenu Items(this NativeMenu menu, params NativeMenuItemBase[] items)
    {
        foreach (var item in items)
            menu.Items.Add(item);

        return menu;
    }

    /// <summary>
    /// Adds items to NativeMenuItem and returns the menu item for method chaining
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    public static NativeMenuItem Items(this NativeMenuItem menu, params NativeMenuItemBase[] items)
    {
        menu.Menu ??= [];
        foreach (var item in items)
            menu.Menu.Items.Add(item);

        return menu;
    }
}