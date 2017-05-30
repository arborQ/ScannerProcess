using RepositoryServices;
using RepositoryServices.Models;
using WorkflowService.Models;

namespace WorkflowService.Providers
{
    internal abstract class BaseCodeProvider
    {
        protected readonly ApplicationService ApplicationService;
        public BaseCodeProvider(ApplicationService applicationService)
        {
            ApplicationService = applicationService;
        }

        public abstract string InitMessage
        {
            get;
        }

        public Machine ProvidedValue
        {
            get; protected set;
        }

        public void Clear()
        {
            ProvidedValue = null;
        }

        protected int SelectedMode => ApplicationService.SettingsRepository.Get().SelectedMode;

        public abstract bool IsEnabled();

        protected abstract Machine LoadMachine(BarCodeModel barCodeModel);

        public bool LoadValue(BarCodeModel barCodeModel)
        {
            ProvidedValue = LoadMachine(barCodeModel);// _applicationService.MachineRepository.GetRecords(r => r.Code == barCodeModel.InputValue).SingleOrDefault();
            return ProvidedValue != null;
        }
    }
}
