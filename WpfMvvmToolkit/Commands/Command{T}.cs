using System;
using System.Windows.Input;

namespace WpfMvvmToolkit.Commands
{
    public class Command<T> : ICommand
    {
        private readonly Action<T?> _executeDelegate;
        private readonly Func<T?, bool> _canExecuteDelegate;

        public event EventHandler? CanExecuteChanged;

        public Command(Action<T?> executeDelegate)
            : this(executeDelegate, (t) => true)
        {

        }

        public Command(Action<T?> executeDelegate, Func<T?, bool> canExecuteDelegate)
        {
            ArgumentNullException.ThrowIfNull(executeDelegate);
            ArgumentNullException.ThrowIfNull(canExecuteDelegate);

            _executeDelegate = executeDelegate;
            _canExecuteDelegate = canExecuteDelegate;
        }

        public bool CanExecute(object? parameter)
        {
            if (!TryGetCommandArgument(parameter, out var argument))
            {
                throw GetArgumentException(parameter);
            }

            return _canExecuteDelegate(argument);
        }

        public void Execute(object? parameter)
        {
            if (!TryGetCommandArgument(parameter, out var argument))
            {
                throw GetArgumentException(parameter);
            }

            _executeDelegate(argument);
        }

        private static ArgumentException GetArgumentException(object? parameter)
        {
            return new ArgumentException($"The parameter must be of type {typeof(T)}.");
        }

        private static bool TryGetCommandArgument(object? parameter, out T? commandArgument)
        {
            if (parameter == null && default(T) is null)
            {
                commandArgument = default(T);
                return true;
            }

            if (parameter is T argument)
            {
                commandArgument = argument;
                return true;
            }

            commandArgument = default(T);
            return false;
        }
    }
}
