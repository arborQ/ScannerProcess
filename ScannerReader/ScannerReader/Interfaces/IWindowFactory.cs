using System;
using System.Collections.Generic;
using ScannerReader.Windows;
using WorkflowService.Interfaces;

namespace ScannerReader.Interfaces
{
    public interface IWindowFactory
    {
        WorkflowWindow CreateWorkflowWindow();

        AdminOptionsWindow CreateAdminOptionsWindow();

        GetValueWindow CreateGetValueWindow(string expectedValue, DialogPosition position = DialogPosition.Center);

        WorkInProgress WorkInProgress(IEnumerable<Func<string>> listOfActions);

        AdminPasswordWindow AdminPasswordWindow();
    }
}