using ScannerReader.Windows;

namespace ScannerReader.Interfaces
{
    public interface IWindowFactory
    {
        WorkflowWindow CreateWorkflowWindow();

        AdminOptionsWindow CreateAdminOptionsWindow();

        GetValueWindow CreateGetValueWindow(string expectedValue);
    }
}