using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryServices.Models;
using WorkflowService.Interfaces;
using WorkflowService.Models;
using WorkflowService.Resources;
using RepositoryServices;

namespace WorkflowService.Providers
{
    internal class TableCodeProvider : BaseCodeProvider, IOrderCodeProvider
    {
        public TableCodeProvider(ApplicationService applicationService) : base(applicationService)
        {
        }

        public override string InitMessage => StateResources.PendingInitMessageTable;

        public override bool IsEnabled()
        {
            return SelectedMode == 1 || SelectedMode == 2;
        }

        protected override Machine LoadMachine(BarCodeModel barCodeModel)
        {
            return ApplicationService.MachineRepository.GetRecords(r => r.Code == barCodeModel.SecondPart).SingleOrDefault();
        }
    }
}
