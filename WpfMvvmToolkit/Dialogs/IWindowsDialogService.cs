using System;
using WpfMvvmToolkit.Windows;

namespace WpfMvvmToolkit.Dialogs;

public interface IWindowsDialogService
{
    event EventHandler? BeforeShowingDialog;
    event EventHandler? AfterShowingDialog;

    void ShowOkErrorMessageBox(string message, string caption = "Error", IWindowViewModel? owner = null);

    void ShowOkInfoMessageBox(string message, string caption = "Information", IWindowViewModel? owner = null);

    void ShowOkWarningMessageBox(string message, string caption = "Warning", IWindowViewModel? owner = null);

    string? ShowOpenFileDialog(string filter = "All files (*.*) | *.*", string? initialDirectoryPath = null, string? fileName = null, IWindowViewModel? owner = null);

    string[] ShowOpenMultipleFilesDialog(string filter = "All files (*.*) | *.*", string? initialDirectoryPath = null, IWindowViewModel? owner = null);

    bool ShowYesNoErrorMessageBox(string message, string caption = "Error", IWindowViewModel? owner = null);

    bool ShowYesNoInfoMessageBox(string message, string caption = "Information", IWindowViewModel? owner = null);

    bool ShowYesNoWarningMessageBox(string message, string caption = "Warning", IWindowViewModel? owner = null);

    string? ShowSaveFileDialog(string filter = "All files (*.*) | *.*", string? initialDirectoryPath = null, string? fileName = null, IWindowViewModel? owner = null);

    string? ShowSelectDirectoryDialog(string? initialDirectoryPath = null);
}
