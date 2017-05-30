using System.Linq;
using RepositoryServices.Models;
using WorkflowService.Interfaces;
using WorkflowService.Models;
using RepositoryServices;
using WorkflowService.Resources;

namespace WorkflowService.Providers
{
    internal class OrderCodeProvider : BaseCodeProvider, IOrderCodeProvider
    {
        public OrderCodeProvider(ApplicationService applicationService) : base(applicationService)
        {
        }

        public override string InitMessage => StateResources.PendingInitMessage;

        public override bool IsEnabled()
        {
            return SelectedMode == 0 || SelectedMode == 2;
        }

        protected override Machine LoadMachine(BarCodeModel barCodeModel)
        {
            return ApplicationService.MachineRepository.GetRecords(r => r.Code == barCodeModel.InputValue).SingleOrDefault();
        }
    }
}
