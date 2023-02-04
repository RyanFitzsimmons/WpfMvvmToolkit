using System;
using System.Windows.Input;

namespace WpfMvvmToolkit.Commands
{
    public class Command : ICommand
    {
        private readonly Action _executeDelegate;
        private readonly Func<bool> _canExecuteDelegate;

        public event EventHandler? CanExecuteChanged;

        public Command(Action executeDelegate)
            : this(executeDelegate, () => true)
        {

        }

        public Command(Action executeDelegate, Func<bool> canExecuteDelegate)
        {
            ArgumentNullException.ThrowIfNull(executeDelegate);
            ArgumentNullException.ThrowIfNull(canExecuteDelegate);

            _executeDelegate = executeDelegate;
            _canExecuteDelegate = canExecuteDelegate;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecuteDelegate();
        }

        public void Execute(object? parameter)
        {
            _executeDelegate();
        }

        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
