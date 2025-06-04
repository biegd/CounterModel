using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CounterApp.Models;

namespace CounterApp.ViewModels
{
    public class CounterViewModel : INotifyPropertyChanged
    {
        private CounterModel _counter = new();
        public event PropertyChangedEventHandler PropertyChanged;

        public int Count
        {
            get => _counter.Count;
            set
            {
                if (_counter.Count != value)
                {
                    _counter.Count = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand IncrementCommand { get; }

        public CounterViewModel()
        {
            IncrementCommand = new RelayCommand(o => Count++);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public event EventHandler CanExecuteChanged { add { } remove { } }

        public void Execute(object parameter) => _execute(parameter);
    }
}
