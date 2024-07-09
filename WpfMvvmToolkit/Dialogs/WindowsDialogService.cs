using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using WpfMvvmToolkit.Configuration;
using WpfMvvmToolkit.Windows;

namespace WpfMvvmToolkit.Dialogs;

public class WindowsDialogService : IWindowsDialogService
{
    private IWindowFactory _windowFactory;

    public WindowsDialogService(IWindowFactory windowFactory)
    {
        _windowFactory = windowFactory;
    }

    public event EventHandler? BeforeShowingDialog;
    public event EventHandler? AfterShowingDialog;

    private Window? GetDialogOwnerWindow(IWindowViewModel? owner)
    {
        if (owner == null)
        {
            return _windowFactory.GetMainWindow();
        }

        if (_windowFactory is not WindowFactory wf)
        {
            throw new NotSupportedException("Owner not supported");
        }

        return wf.GetWindows(owner).FirstOrDefault();
    }

    public string? ShowSelectDirectoryDialog(string? initialDirectoryPath = null)
    {
        BeforeShowingDialog?.Invoke(this, EventArgs.Empty);

        using var dialog = new System.Windows.Forms.FolderBrowserDialog
        {
            Description = "Directory",
            UseDescriptionForTitle = true,
            SelectedPath = initialDirectoryPath ?? "",
            ShowNewFolderButton = true
        };

        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            AfterShowingDialog?.Invoke(this, EventArgs.Empty);
            return dialog.SelectedPath;
        }

        AfterShowingDialog?.Invoke(this, EventArgs.Empty);
        return null;
    }

    public void ShowOkErrorMessageBox(string message, string caption = "Error", IWindowViewModel? owner = null)
    {
        BeforeShowingDialog?.Invoke(this, EventArgs.Empty);
        MessageBox.Show(
            GetDialogOwnerWindow(owner),
            message,
            caption,
            MessageBoxButton.OK,
            MessageBoxImage.Error);
        AfterShowingDialog?.Invoke(this, EventArgs.Empty);
    }

    public void ShowOkInfoMessageBox(string message, string caption = "Information", IWindowViewModel? owner = null)
    {
        BeforeShowingDialog?.Invoke(this, EventArgs.Empty);
        MessageBox.Show(
            GetDialogOwnerWindow(owner),
            message,
            caption,
            MessageBoxButton.OK,
            MessageBoxImage.Information);
        AfterShowingDialog?.Invoke(this, EventArgs.Empty);
    }

    public void ShowOkWarningMessageBox(string message, string caption = "Warning", IWindowViewModel? owner = null)
    {
        BeforeShowingDialog?.Invoke(this, EventArgs.Empty);
        MessageBox.Show(
            GetDialogOwnerWindow(owner),
            message,
            caption,
            MessageBoxButton.OK,
            MessageBoxImage.Warning);
        AfterShowingDialog?.Invoke(this, EventArgs.Empty);
    }

    public string? ShowOpenFileDialog(string filter = "All files (*.*) | *.*", string? initialDirectoryPath = null, IWindowViewModel? owner = null)
    {
        BeforeShowingDialog?.Invoke(this, EventArgs.Empty);
        var dialog = new OpenFileDialog
        {
            InitialDirectory = initialDirectoryPath,
            Filter = filter,
        };

        if (dialog.ShowDialog(GetDialogOwnerWindow(owner)) == true)
        {
            AfterShowingDialog?.Invoke(this, EventArgs.Empty);
            return dialog.FileName;
        }
        else
        {
            AfterShowingDialog?.Invoke(this, EventArgs.Empty);
            return null;
        }
    }

    public string[] ShowOpenMultipleFilesDialog(string filter = "All files (*.*) | *.*", string? initialDirectoryPath = null, IWindowViewModel? owner = null)
    {
        BeforeShowingDialog?.Invoke(this, EventArgs.Empty);
        var dialog = new OpenFileDialog
        {
            InitialDirectory = initialDirectoryPath,
            Filter = filter,
            Multiselect = true,
        };

        var result = dialog.ShowDialog(GetDialogOwnerWindow(owner));

        if (result == true)
        {
            AfterShowingDialog?.Invoke(this, EventArgs.Empty);
            return dialog.FileNames;
        }
        else
        {
            AfterShowingDialog?.Invoke(this, EventArgs.Empty);
            return Array.Empty<string>();
        }
    }

    public string? ShowSaveFileDialog(string filter = "All files (*.*) | *.*", string? initialDirectoryPath = null, IWindowViewModel? owner = null)
    {
        BeforeShowingDialog?.Invoke(this, EventArgs.Empty);
        var dialog = new SaveFileDialog
        {
            InitialDirectory = initialDirectoryPath,
            Filter = filter,
        };

        if (dialog.ShowDialog(GetDialogOwnerWindow(owner)) == true)
        {
            AfterShowingDialog?.Invoke(this, EventArgs.Empty);
            return dialog.FileName;
        }
        else
        {
            AfterShowingDialog?.Invoke(this, EventArgs.Empty);
            return null;
        }
    }

    public bool ShowYesNoErrorMessageBox(string message, string caption = "Error", IWindowViewModel? owner = null)
    {
        BeforeShowingDialog?.Invoke(this, EventArgs.Empty);
        var result = MessageBox.Show(
            GetDialogOwnerWindow(owner),
            message,
            caption,
            MessageBoxButton.YesNo,
            MessageBoxImage.Error) == MessageBoxResult.Yes;
        AfterShowingDialog?.Invoke(this, EventArgs.Empty);
        return result;
    }

    public bool ShowYesNoInfoMessageBox(string message, string caption = "Information", IWindowViewModel? owner = null)
    {
        BeforeShowingDialog?.Invoke(this, EventArgs.Empty);
        var result = MessageBox.Show(
            GetDialogOwnerWindow(owner),
            message,
            caption,
            MessageBoxButton.YesNo,
            MessageBoxImage.Information) == MessageBoxResult.Yes;
        AfterShowingDialog?.Invoke(this, EventArgs.Empty);
        return result;
    }

    public bool ShowYesNoWarningMessageBox(string message, string caption = "Warning", IWindowViewModel? owner = null)
    {
        BeforeShowingDialog?.Invoke(this, EventArgs.Empty);
        var result = MessageBox.Show(
            GetDialogOwnerWindow(owner),
            message,
            caption,
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning) == MessageBoxResult.Yes;
        AfterShowingDialog?.Invoke(this, EventArgs.Empty);
        return result;
    }
}
