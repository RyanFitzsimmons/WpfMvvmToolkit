using Microsoft.Win32;
using System;
using System.Windows;

namespace WpfMvvmToolkit.Dialogs;

public class WindowsDialogService : IWindowsDialogService
{
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

    public void ShowOkErrorMessageBox(string message, string caption = "Error")
    {
        MessageBox.Show(
                message,
                caption,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
    }

    public void ShowOkInfoMessageBox(string message, string caption = "Information")
    {
        MessageBox.Show(
                message,
                caption,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
    }

    public void ShowOkWarningMessageBox(string message, string caption = "Warning")
    {
        MessageBox.Show(
                message,
                caption,
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
    }

    public string? ShowOpenFileDialog(string filter = "All files (*.*) | *.*", string? initialDirectoryPath = null)
    {
        var dialog = new OpenFileDialog
        {
            InitialDirectory = initialDirectoryPath,
            Filter = filter,
        };
        if (dialog.ShowDialog() == true) return dialog.FileName;
        else return null;
    }

    public string[] ShowOpenMultipleFilesDialog(string filter = "All files (*.*) | *.*", string? initialDirectoryPath = null)
    {
        var dialog = new OpenFileDialog
        {
            InitialDirectory = initialDirectoryPath,
            Filter = filter,
            Multiselect = true,
        };

        return dialog.ShowDialog() == true ? dialog.FileNames : Array.Empty<string>();
    }

    public string? ShowSaveFileDialog(string filter = "All files (*.*) | *.*", string? initialDirectoryPath = null)
    {
        var dialog = new SaveFileDialog
        {
            InitialDirectory = initialDirectoryPath,
            Filter = filter,
        };
        if (dialog.ShowDialog() == true) return dialog.FileName;
        else return null;
    }

    public bool ShowYesNoErrorMessageBox(string message, string caption = "Error")
    {
        return MessageBox.Show(
                message,
                caption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Error) == MessageBoxResult.Yes;
    }

    public bool ShowYesNoInfoMessageBox(string message, string caption = "Information")
    {
        return MessageBox.Show(
                message,
                caption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Information) == MessageBoxResult.Yes;
    }

    public bool ShowYesNoWarningMessageBox(string message, string caption = "Warning")
    {
        return MessageBox.Show(
                message,
                caption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes;
    }
}
