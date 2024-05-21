using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfMvvmToolkit.Commands;

public class AsyncCommand<T> : ICommand
{
    private readonly Func<T?, Task> _executeDelegate;
    private readonly Func<T?, bool> _canExecuteDelegate;
    private readonly Action<Exception> _exceptionDelegate;

    public event EventHandler? CanExecuteChanged;

    public AsyncCommand(Func<T?, Task> executeDelegate)
        : this(executeDelegate, (t) => true, msg => Debug.WriteLine(msg))
    {

    }

    /// <summary>
    /// Provides a way to log any exceptions thrown by the async void Execute method.
    /// </summary>
    public AsyncCommand(Func<T?, Task> executeDelegate, Func<T?, bool> canExecuteDelegate, Action<Exception> exceptionDelegate)
    {
        ArgumentNullException.ThrowIfNull(executeDelegate);
        ArgumentNullException.ThrowIfNull(canExecuteDelegate);

        _executeDelegate = executeDelegate;
        _canExecuteDelegate = canExecuteDelegate;
        _exceptionDelegate = exceptionDelegate;
    }

    public bool CanExecute(object? parameter)
    {
        if (!Command<T>.TryGetCommandArgument(parameter, out var argument))
        {
            throw Command<T>.GetArgumentException(parameter);
        }

        return _canExecuteDelegate(argument);
    }

    public async void Execute(object? parameter)
    {
        try
        {
            if (!Command<T>.TryGetCommandArgument(parameter, out var argument))
            {
                throw Command<T>.GetArgumentException(parameter);
            }

            await _executeDelegate(argument);
        }
        catch (Exception e)
        {
            _exceptionDelegate.Invoke(e);
        }
    }

    public void NotifyCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
