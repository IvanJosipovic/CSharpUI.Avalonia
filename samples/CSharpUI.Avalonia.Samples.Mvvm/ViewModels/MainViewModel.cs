using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CSharpUI.Avalonia.Samples.Mvvm.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private string _greeting = "Hello from the view model";
    private int _clickCount;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Greeting
    {
        get => _greeting;
        set
        {
            if (_greeting == value)
            {
                return;
            }

            _greeting = value;
            OnPropertyChanged();
        }
    }

    public string ClickCountText => $"Button clicked {_clickCount} times";

    public ICommand IncrementCommand { get; }

    public MainViewModel() => IncrementCommand = new IncrementCommandImpl(this);

    private void Increment()
    {
        _clickCount++;
        OnPropertyChanged(nameof(ClickCountText));
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private sealed class IncrementCommandImpl(MainViewModel owner) : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { }
            remove { }
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) => owner.Increment();
    }
}
