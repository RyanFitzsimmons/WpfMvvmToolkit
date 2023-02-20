namespace WpfMvvmToolkit.Dialogs;

public interface IWindowsDialogService
{
    void ShowOkErrorMessageBox(string message, string caption = "Error");

    void ShowOkInfoMessageBox(string message, string caption = "Information");

    void ShowOkWarningMessageBox(string message, string caption = "Warning");

    string? ShowOpenFileDialog(string filter = "All files (*.*) | *.*", string? initialDirectoryPath = null);

    string[] ShowOpenMultipleFilesDialog(string filter = "All files (*.*) | *.*", string? initialDirectoryPath = null);

    bool ShowYesNoErrorMessageBox(string message, string caption = "Error");

    bool ShowYesNoInfoMessageBox(string message, string caption = "Information");

    bool ShowYesNoWarningMessageBox(string message, string caption = "Warning");

    string? ShowSaveFileDialog(string filter = "All files (*.*) | *.*", string? initialDirectoryPath = null);

    string? ShowSelectDirectoryDialog(string? initialDirectoryPath = null);
}
