using System;
using System.Collections.Generic;
using ScannerReader.Windows;

namespace ScannerReader.Interfaces
{
    public interface IWindowFactory
    {
        WorkflowWindow CreateWorkflowWindow();

        AdminOptionsWindow CreateAdminOptionsWindow();

        GetValueWindow CreateGetValueWindow(string expectedValue);

        WorkInProgress WorkInProgress(IEnumerable<Func<string>> listOfActions);

        AdminPasswordWindow AdminPasswordWindow();
    }
}