using RepositoryServices.Models;
using WorkflowService.Models;

namespace WorkflowService.Interfaces
{
    public interface IOrderCodeProvider
    {
        Machine ProvidedValue { get; }

        bool IsEnabled();

        bool LoadValue(BarCodeModel barCodeModel);

        string InitMessage { get; }

        void Clear();
    }
}
