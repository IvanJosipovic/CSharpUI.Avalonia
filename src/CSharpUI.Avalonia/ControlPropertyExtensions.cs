using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data.Converters;
using Avalonia.Styling;
using CSharpUI.Avalonia;
using CSharpUI.Avalonia.Helpers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AvaloniaToolTip = Avalonia.Controls.ToolTip;

namespace CSharpUI.Avalonia;

/// <summary>
/// Extensions for setting Avalonia properties in a fluent way
/// </summary>
public static class ControlPropertyExtensions
{
    /// <summary>
    /// Used to execute action that sets multiple properties on control
    /// </summary>
    /// <typeparam name="TControl"></typeparam>
    /// <param name="control"></param>
    /// <param name="setAction"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TControl _set<TControl>(this TControl control, Action setAction)
    {
        setAction();
        return control;
    }

    /// <summary>
    /// Used to bind one avalonia property to another
    /// </summary>
    /// <typeparam name="TControl"></typeparam>
    /// <param name="control"></param>
    /// <param name="avaloniaProperty"></param>
    /// <param name="propertyToBindTo"></param>
    /// <param name="bindingMode"></param>
    /// <param name="converter"></param>
    /// <param name="overrideView"></param>
    /// <returns></returns>
    [RequiresUnreferencedCode("Uses Binding which uses Reflection")]
    public static TControl _set<TControl>(this TControl control, AvaloniaProperty avaloniaProperty,
        AvaloniaProperty propertyToBindTo, BindingMode? bindingMode, IValueConverter? converter, ViewBase? overrideView)
        where TControl : AvaloniaObject
    {
        var binding = new Binding()
        {
            Path = propertyToBindTo.Name,
            Mode = bindingMode ?? BindingMode.Default,
            Converter = converter
        };

        control[!avaloniaProperty] = binding;
        return control;
    }

    /// <summary>
    /// Used to pass Binding object constructed by end-user
    /// </summary>
    /// <typeparam name="TControl"></typeparam>
    /// <param name="control"></param>
    /// <param name="avaloniaProperty"></param>
    /// <param name="binding"></param>
    /// <returns></returns>
    public static TControl _set<TControl>(this TControl control, AvaloniaProperty avaloniaProperty, IBinding binding)
        where TControl : AvaloniaObject
    {
        control[!avaloniaProperty] = binding;
        return control;
    }

    /// <summary>
    /// Converts Color to Brush
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Brush ToBrush(this Color color) => new SolidColorBrush(color);

    /// <summary>
    /// Shortcut for DockPanel.Dock (in xaml: DockPanel.Dock) extension
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="control"></param>
    /// <param name="dock"></param>
    /// <returns></returns>
    public static TElement Dock<TElement>(this TElement control, Dock dock)
        where TElement : Control
    {
        DockPanel.SetDock(control, dock);
        return control;
    }

    /// <summary>
    /// Shortcut for Grid_Column (in xaml: Grid.Column) extension
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="control">Control for positioning</param>
    /// <param name="value">Grid.Column value</param>
    /// <returns></returns>
	public static TElement Col<TElement>(this TElement control, int value)
        where TElement : Control
    {
        Grid.SetColumn(control, value);
        return control;
    }

    /// <summary>
    /// Shortcut for Grid_Row (in xaml: Grid.Row) extension
    /// </summary>
    /// <typeparam name="TElement">Control type</typeparam>
    /// <param name="control">Control for positioning</param>
    /// <param name="value">Grid.Row value</param>
    /// <returns></returns>
    public static TElement Row<TElement>(this TElement control, int value)
        where TElement : Control
    {
        Grid.SetRow(control, value);
        return control;
    }

    /// <summary>
    /// Shortcut for Grid_ColumnDefinitions (in xaml: Grid.ColumnDefinitions) extension
    /// </summary>
    /// <typeparam name="TElement">Grid</typeparam>
    /// <param name="control">Grid control</param>
    /// <param name="value">Grid.ColumnDefinitions value</param>
    /// <returns></returns>
	public static TElement Cols<TElement>(this TElement control, ColumnDefinitions value)
        where TElement : Grid
    {
        control.ColumnDefinitions = value;
        return control;
    }

    /// <summary>
    /// Shortcut for Grid_RowDefinitions (in xaml: Grid.RowDefinitions) extension
    /// </summary>
    /// <typeparam name="TElement">Grid</typeparam>
    /// <param name="control">Grid control</param>
    /// <param name="value">Grid.RowDefinitions value</param>
    /// <returns></returns>
	public static TElement Rows<TElement>(this TElement control, RowDefinitions value)
        where TElement : Grid
    {
        control.RowDefinitions = value;
        return control;
    }

    /// <summary>
    /// Shortcut for Grid_ColumnDefinitions (in xaml: Grid.ColumnDefinitions) extension
    /// </summary>
    /// <typeparam name="TElement">Grid</typeparam>
    /// <param name="control">Grid control</param>
    /// <param name="value">String representing ColumnDefinitions i.e. "0,*,30,Auto" </param>
    /// <returns></returns>
	public static TElement Cols<TElement>(this TElement control, string value)
        where TElement : Grid
    {
        control.ColumnDefinitions = ColumnDefinitions.Parse(value);
        return control;
    }

    /// <summary>
    /// Shortcut for Grid_RowDefinitions (in xaml: Grid.RowDefinitions) extension
    /// </summary>
    /// <typeparam name="TElement">Grid</typeparam>
    /// <param name="control">Grid control</param>
    /// <param name="value">String representing RowDefinitions i.e. "0,*,30,Auto" </param>
    /// <returns></returns>
	public static TElement Rows<TElement>(this TElement control, string value)
        where TElement : Grid
    {
        control.RowDefinitions = RowDefinitions.Parse(value);
        return control;
    }

    /// <summary>
    /// Shortcut for Grid_ColumnSpan (in xaml: Grid.ColumnSpan) extension
    /// </summary>
    /// <typeparam name="TElement">Control Type</typeparam>
    /// <param name="control">Control for positioning</param>
    /// <param name="value">Grid.ColumnSpan value</param>
    /// <returns></returns>
	public static TElement ColSpan<TElement>(this TElement control, int value)
        where TElement : Control
    {
        Grid.SetColumnSpan(control, value);
        return control;
    }

    /// <summary>
    /// Shortcut for Grid_RowSpan (in xaml: Grid.RowSpan) extension
    /// </summary>
    /// <typeparam name="TElement">Control type</typeparam>
    /// <param name="control">Control for positioning</param>
    /// <param name="value">Grid.RowSpan value</param>
    /// <returns></returns>
	public static TElement RowSpan<TElement>(this TElement control, int value)
        where TElement : Control
    {
        Grid.SetRowSpan(control, value);
        return control;
    }

    /// <summary>
    /// Adds children to Panel and returns the panel for method chaining
    /// </summary>
    /// <typeparam name="TPanel"></typeparam>
    /// <param name="container"></param>
    /// <param name="children"></param>
    /// <returns></returns>
    public static TPanel Children<TPanel>(this TPanel container, params Control[] children)
        where TPanel : Panel
    {
        foreach (var child in children)
            container.Children.Add(child);
        return container;
    }

    /// <summary>
    /// Adds items to ItemsControl and returns the control for method chaining
    /// </summary>
    /// <typeparam name="TItemsControl"></typeparam>
    /// <param name="container"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    public static TItemsControl Items<TItemsControl>(this TItemsControl container, params AvaloniaObject[] items)
        where TItemsControl : ItemsControl
    {
        if (container.Items is IList itemsCollection)
            foreach (var item in items)
                itemsCollection.Add(item);
        return container;
    }

    /// <summary>
    /// Sets ItemTemplate for TabControl
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="control"></param>
    /// <param name="build"></param>
    /// <returns></returns>
    public static TabControl ItemTemplate<TItem>(this TabControl control, Func<TItem, Control> build) =>
        control.ItemTemplate<TItem, TabControl>(build);

    /// <summary>
    /// Sets ItemTemplate for TabControl
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="control"></param>
    /// <param name="build"></param>
    /// <returns></returns>
    public static SelectingItemsControl ItemTemplate<TItem>(this SelectingItemsControl control,
        Func<TItem, Control> build) =>
        control.ItemTemplate<TItem, SelectingItemsControl>(build);

    //public static ItemsControl ItemTemplate<TItem>(this ItemsControl control, Func<TItem, Control> build) =>
    //	ItemTemplate<TItem, ItemsControl>(control, build);

    /// <summary>
    /// Sets ItemTemplate for ItemsControl and its descendants
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TItemsControl"></typeparam>
    /// <param name="control"></param>
    /// <param name="build"></param>
    /// <returns></returns>
    public static TItemsControl ItemTemplate<TItem, TItemsControl>(this TItemsControl control,
        Func<TItem, Control> build)
        where TItemsControl : ItemsControl
    {
        control.ItemTemplate = control.ItemTemplate = new FuncDataTemplate<TItem>((val, _) => build(val));
        return control;
    }

    /// <summary>
    /// Sets ItemTemplate for MenuFlyout
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="control"></param>
    /// <param name="build"></param>
    /// <returns></returns>
    public static MenuFlyout ItemTemplate<TItem>(this MenuFlyout control, Func<TItem, Control> build)
    {
        control.ItemTemplate = new FuncDataTemplate<TItem>((val, _) => build(val));
        return control;
    }

    /// <summary>
    /// Sets ItemTemplate for MenuItem
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="control"></param>
    /// <param name="build"></param>
    /// <returns></returns>
    public static MenuItem ItemTemplate<TItem>(this MenuItem control, Func<TItem, Control> build)
    {
        control.ItemTemplate = new FuncDataTemplate<TItem>((val, _) => build(val));
        return control;
    }

    /// <summary>
    /// Sets ItemTemplate for Menu
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="control"></param>
    /// <param name="build"></param>
    /// <returns></returns>
    public static Menu ItemTemplate<TItem>(this Menu control, Func<TItem, Control> build)
    {
        control.ItemTemplate = new FuncDataTemplate<TItem>((val, _) => build(val));
        return control;
    }

    /// <summary>
    /// Sets ItemsPanel for ItemsControl
    /// </summary>
    /// <typeparam name="TItemsControl"></typeparam>
    /// <param name="control"></param>
    /// <param name="panel"></param>
    /// <returns></returns>
    public static TItemsControl ItemsPanel<TItemsControl>(this TItemsControl control, Panel panel)
        where TItemsControl : ItemsControl
    {
        control.ItemsPanel = new PanelTemplate(panel);
        return control;
    }

    record PanelTemplate(Panel panel) : ITemplate<Panel?>
    {
        public Panel Build() => panel;
        object ITemplate.Build() => throw new NotImplementedException();
    }

    /// <summary>
    /// Used to execute action that sets multiple properties on control
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="control"></param>
    /// <param name="process"></param>
    /// <returns></returns>
    public static TElement With<TElement>(this TElement control, Action<TElement> process)
    {
        process?.Invoke(control);
        return control;
    }


    /// <summary>
    /// Sets control Name and registers it in the specified INameScope
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="control"></param>
    /// <param name="name"></param>
    /// <param name="ns"></param>
    /// <returns></returns>
    public static TElement Name<TElement>(this TElement control, string name, INameScope ns)
        where TElement : Control
    {
        ns?.Register(name, control);
        control.Name = name;
        return control;
    }

    /// <summary>
    /// Adds styles to control and returns the control for method chaining
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="control"></param>
    /// <param name="styles"></param>
    /// <returns></returns>
    public static TElement Styles<TElement>(this TElement control, params Style[] styles)
        where TElement : Control
    {
        foreach (var style in styles)
            control.Styles.Add(style);

        return control;
    }

    /// <summary>
    /// Adds styles to control and returns the control for method chaining
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="control"></param>
    /// <param name="styles"></param>
    /// <returns></returns>
    public static TElement Styles<TElement>(this TElement control, IEnumerable<Style> styles)
        where TElement : Control
    {
        foreach (var style in styles)
            control.Styles.Add(style);

        return control;
    }

    /// <summary>
    /// Adds class to control and returns the control for method chaining
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="control"></param>
    /// <param name="className"></param>
    /// <param name="line"></param>
    /// <param name="caller"></param>
    /// <returns></returns>
    public static TElement Class<TElement>(this TElement control, string className, [CallerLineNumber] int line = 0,
        [CallerMemberName] string? caller = default)
        where TElement : Control
    {
        control.Classes.Add(className);
        return control;
    }

    //[RequiresUnreferencedCode("Uses Binding which depends on Reflection")]
    //public static TElement BindClass<TElement>(this TElement control, bool value, string className,
    //    object? bindingSource = null, [CallerLineNumber] int line = 0, [CallerMemberName] string? caller = default,
    //    [CallerArgumentExpression(nameof(value))] string? ps = null)
    //    where TElement : Control
    //{
    //    var path = PropertyPathHelper.GetNameFromPropertyPath(ps);
    //    var binding = new Binding(path, BindingMode.OneWay);

    //    if (bindingSource != null)
    //        binding.Source = bindingSource;

    //    control.BindClass(className, binding, null!);
    //    return control;
    //}

    //public static StackTrace GetDeeperStackTrace(int depth) =>
    //    depth > 0 ? GetDeeperStackTrace(depth - 1) : new StackTrace(0, true);


    /// <summary>
    /// Adds DataTemplates to control and returns the control for method chaining
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="control"></param>
    /// <param name="dataTemplate"></param>
    /// <returns></returns>
    public static TElement DataTemplates<TElement>(this TElement control, params IDataTemplate[] dataTemplate)
        where TElement : Control
    {
        foreach (var template in dataTemplate)
            control.DataTemplates.Add(template);
        return control;
    }

    /// <summary>
    /// Sets AvaloniaProperty value or binding to control and returns the control for method chaining
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="control"></param>
    /// <param name="property"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TElement SetProp<TElement, TValue>(this TElement control, AvaloniaProperty property,
        TValue value)
        where TElement : Control
    {
        if (value is IBinding binding)
        {
            control[!property] = binding;
        }
        else
        {
            control[property] = value;
        }

        return control;
    }

    /// <summary>
    /// Sets ToolTip value or binding to control and returns the control for method chaining
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="control"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TElement ToolTip<TElement, TValue>(this TElement control, TValue value)
        where TElement : Control
    {
        var prop = AvaloniaToolTip.TipProperty;
        if (value is IBinding binding)
        {
            control[!prop] = binding;
        }
        else
        {
            control[prop] = value;
        }

        return control;
    }

    /// <summary>
    /// Adds flyout to button and activates it on button click
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="control">target button</param>
    /// <param name="flyout">flyout to activate</param>
    /// <returns></returns>
    public static TElement AddFlyoutOnClick<TElement>(this TElement control, FlyoutBase flyout) where TElement : Button
    {
        control.Click += (_, _) => flyout.ShowAt(control);
        //control.OnClick(_ => flyout.ShowAt(control));
        return control;
    }

    /// <summary>
    /// Adds a menu item to the specified menu flyout.
    /// </summary>
    /// <typeparam name="TElement">The type of the menu flyout element.</typeparam>
    /// <param name="menuFlyout">The menu flyout to which the item will be added.</param>
    /// <param name="menuItem">The menu item to be added to the flyout.</param>
    /// <returns>The menu flyout with the added item.</returns>
    public static TElement? AddItem<TElement>(this TElement menuFlyout, MenuItem menuItem)
        where TElement : MenuFlyout
    {
        (menuFlyout?.Items)?.Add(menuItem);
        return menuFlyout;
    }

    /// <summary>
    /// Adds item to MenuFlyout
    /// </summary>
    /// <typeparam name="TElement">MenuFlyout type</typeparam>
    /// <param name="menuFlyout">Target MenuFlyout control</param>
    /// <param name="text">Item text</param>
    /// <param name="command">Item command</param>
    /// <param name="commandParameter">Command parameter</param>
    /// <returns></returns>
    public static TElement? AddItem<TElement>(this TElement menuFlyout, string text, ICommand command,
        object? commandParameter = null)
        where TElement : MenuFlyout
    {
        var item = new MenuItem() { Header = text, Command = command };
        if (commandParameter != null)
            item.CommandParameter = commandParameter;

        (menuFlyout?.Items)?.Add(item);
        return menuFlyout;
    }

    /// <summary>
    /// Sets control instance reference to field so it can be accessed later in Markup
    /// </summary>
    /// <typeparam name="TElement">Control Type</typeparam>
    /// <param name="control">Control instance</param>
    /// <param name="field">field that will accept reference to control</param>
    /// <returns></returns>
    public static TElement Ref<TElement>(this TElement control, out TElement field)
    {
        field = control;
        return control;
    }
}