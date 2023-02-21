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

    private Window? GetDialogOwnerWindow(IWindowViewModel? owner)
    {
        if (owner == null)
        {
            return null;
        }

        if (_windowFactory is not WindowFactory wf)
        {
            throw new NotSupportedException("Owner not supported");
        }

        return wf.GetWindows(owner).FirstOrDefault();
    }

    public string? ShowSelectDirectoryDialog(string? initialDirectoryPath = null)
    {
        using var dialog = new System.Windows.Forms.FolderBrowserDialog
        {
            Description = "Directory",
            UseDescriptionForTitle = true,
            SelectedPath = initialDirectoryPath,
            ShowNewFolderButton = true
        };

        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            return dialog.SelectedPath;
        }

        return null;
    }

    public void ShowOkErrorMessageBox(string message, string caption = "Error", IWindowViewModel? owner = null)
    {
        MessageBox.Show(
            GetDialogOwnerWindow(owner),
            message,
            caption,
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }

    public void ShowOkInfoMessageBox(string message, string caption = "Information", IWindowViewModel? owner = null)
    {
        MessageBox.Show(
            GetDialogOwnerWindow(owner),
            message,
            caption,
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    public void ShowOkWarningMessageBox(string message, string caption = "Warning", IWindowViewModel? owner = null)
    {
        MessageBox.Show(
            GetDialogOwnerWindow(owner),
            message,
            caption,
            MessageBoxButton.OK,
            MessageBoxImage.Warning);
    }

    public string? ShowOpenFileDialog(string filter = "All files (*.*) | *.*", string? initialDirectoryPath = null, IWindowViewModel? owner = null)
    {
        var dialog = new OpenFileDialog
        {
            InitialDirectory = initialDirectoryPath,
            Filter = filter,
        };
        if (dialog.ShowDialog(GetDialogOwnerWindow(owner)) == true) return dialog.FileName;
        else return null;
    }

    public string[] ShowOpenMultipleFilesDialog(string filter = "All files (*.*) | *.*", string? initialDirectoryPath = null, IWindowViewModel? owner = null)
    {
        var dialog = new OpenFileDialog
        {
            InitialDirectory = initialDirectoryPath,
            Filter = filter,
            Multiselect = true,
        };

        return dialog.ShowDialog(GetDialogOwnerWindow(owner)) == true ? dialog.FileNames : Array.Empty<string>();
    }

    public string? ShowSaveFileDialog(string filter = "All files (*.*) | *.*", string? initialDirectoryPath = null, IWindowViewModel? owner = null)
    {
        var dialog = new SaveFileDialog
        {
            InitialDirectory = initialDirectoryPath,
            Filter = filter,
        };
        if (dialog.ShowDialog(GetDialogOwnerWindow(owner)) == true) return dialog.FileName;
        else return null;
    }

    public bool ShowYesNoErrorMessageBox(string message, string caption = "Error", IWindowViewModel? owner = null)
    {
        return MessageBox.Show(
            GetDialogOwnerWindow(owner),
            message,
            caption,
            MessageBoxButton.YesNo,
            MessageBoxImage.Error) == MessageBoxResult.Yes;
    }

    public bool ShowYesNoInfoMessageBox(string message, string caption = "Information", IWindowViewModel? owner = null)
    {
        return MessageBox.Show(
            GetDialogOwnerWindow(owner),
            message,
            caption,
            MessageBoxButton.YesNo,
            MessageBoxImage.Information) == MessageBoxResult.Yes;
    }

    public bool ShowYesNoWarningMessageBox(string message, string caption = "Warning", IWindowViewModel? owner = null)
    {
        return MessageBox.Show(
            GetDialogOwnerWindow(owner),
            message,
            caption,
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning) == MessageBoxResult.Yes;
    }
}
