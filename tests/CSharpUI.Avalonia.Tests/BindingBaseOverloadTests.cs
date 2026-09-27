using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using CSharpUI.Avalonia.Tests;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CSharpUI.Avalonia.Tests.BindingTests;

public sealed class BindingBaseOverloadTests : AvaloniaTestBase
{
    public sealed class Vm : INotifyPropertyChanged
    {
        private string _name = "initial";
        private int _row;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public int Row
        {
            get => _row;
            set { _row = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public sealed class ReflectionBindingView(Vm vm) : ViewBase<Vm>(vm)
    {
        public TextBlock TextBlock = null!;

        protected override object Build(Vm viewModel) =>
            new TextBlock().Ref(out TextBlock).Text(new Binding(nameof(Vm.Name)));
    }

    [AvaloniaFact]
    public void Reflection_binding_is_reactive()
    {
        var vm = new Vm();
        var view = new ReflectionBindingView(vm);
        var window = new Window { Content = view };
        window.Show();
        Dispatcher.UIThread.RunJobs();

        Assert.Equal("initial", view.TextBlock.Text);

        vm.Name = "updated";
        Dispatcher.UIThread.RunJobs();

        Assert.Equal("updated", view.TextBlock.Text);
    }

    [AvaloniaFact]
    public void Explicit_source_binding_is_applied()
    {
        var vm = new Vm { Name = "source value" };
        var text = new TextBlock().Text(new Binding(nameof(Vm.Name)) { Source = vm });
        var window = new Window { Content = text };
        window.Show();
        Dispatcher.UIThread.RunJobs();

        Assert.Equal("source value", text.Text);
    }

    [AvaloniaFact]
    public void Two_way_binding_updates_source()
    {
        var vm = new Vm { Name = "start" };
        var textBox = new TextBox().Text(new Binding(nameof(Vm.Name))
        {
            Source = vm,
            Mode = BindingMode.TwoWay
        });
        var window = new Window { Content = textBox };
        window.Show();
        Dispatcher.UIThread.RunJobs();

        Assert.Equal("start", textBox.Text);
        textBox.Text = "typed";
        Dispatcher.UIThread.RunJobs();
        Assert.Equal("typed", vm.Name);
    }

    [AvaloniaFact]
    public void Attached_property_binding_is_applied()
    {
        var vm = new Vm { Row = 3 };
        var child = new Border().Grid_Row(new Binding(nameof(Vm.Row)) { Source = vm });
        var grid = new Grid().Children(child);
        var window = new Window { Content = grid };
        window.Show();
        Dispatcher.UIThread.RunJobs();

        Assert.Equal(3, Grid.GetRow(child));
    }
}
