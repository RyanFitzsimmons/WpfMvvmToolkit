using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfMvvmToolkit.Commands;

public class AsyncCommand : ICommand
{
    private readonly Func<Task> _executeDelegate;
    private readonly Func<bool> _canExecuteDelegate;
    private readonly Action<Exception> _exceptionDelegate;

    public event EventHandler? CanExecuteChanged;

    public AsyncCommand(Func<Task> executeDelegate)
        : this(executeDelegate, () => true, msg => Debug.WriteLine(msg))
    {

    }

    /// <summary>
    /// Provides a way to log any exceptions thrown by the async void Execute method.
    /// </summary>
    public AsyncCommand(Func<Task> executeDelegate, Func<bool> canExecuteDelegate, Action<Exception> exceptionDelegate)
    {
        ArgumentNullException.ThrowIfNull(executeDelegate);
        ArgumentNullException.ThrowIfNull(canExecuteDelegate);

        _executeDelegate = executeDelegate;
        _canExecuteDelegate = canExecuteDelegate;
        _exceptionDelegate = exceptionDelegate;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecuteDelegate();
    }

    public async void Execute(object? parameter)
    {
        try
        {
            await _executeDelegate();
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