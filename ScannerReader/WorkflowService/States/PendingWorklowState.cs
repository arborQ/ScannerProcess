using System.Linq;
using Logger.Interfaces;
using RepositoryServices;
using WorkflowService.Models;
using WorkflowService.Resources;
using WorkflowService.Interfaces;
using RepositoryServices.Models;

namespace WorkflowService.States
{
    public class PendingWorklowState : WorkflowState
    {
        private readonly ApplicationService _applicationService;
        private readonly ILogService _logService;
        private readonly IOrderCodeProvider[] _codeProviders;
        public PendingWorklowState(IWorkflowOutput workflowOutput,
            IWorkflowStateFactory workflowStateFactory,
            ApplicationService applicationService,
            IOrderCodeProvider[] codeProviders,
            ILogService logService) : base(workflowOutput, workflowStateFactory)
        {
            _applicationService = applicationService;
            _logService = logService;
            _codeProviders = codeProviders;
        }

        public override string Code => "PENDING";

        private IOrderCodeProvider[] EnabledProviders => _codeProviders.Where(p => p.IsEnabled()).ToArray();

        private IOrderCodeProvider ActiveCodeProvider()
        {
            return EnabledProviders.FirstOrDefault(o => o.ProvidedValue == null);
        }

        public override IWorkflowState Initialize()
        {
            WorkflowOutput.Message = ActiveCodeProvider().InitMessage;// StateResources.PendingInitMessage;

            return base.Initialize();
        }

        protected override IWorkflowState Trigger(BarCodeModel inputCode)
        {
            Machine record;
            var codeProvider = ActiveCodeProvider();

            if (!codeProvider.LoadValue(inputCode))
            {
                WorkflowOutput.Message = string.Format(StateResources.UnknownOrderNumberMessageFormat, inputCode.InputValue);
                return this;
            }

            if (EnabledProviders.All(p => p.ProvidedValue != null))
            {
                var codes = EnabledProviders.Select(p => p.ProvidedValue.Code).Distinct().ToList();
                if (codes.Count() != 1)
                {
                    foreach (var provider in EnabledProviders)
                    {
                        provider.Clear();
                    }

                    WorkflowOutput.Message = $"Podane kody nie pasuj¹: {string.Join(",", codes)}";

                    return this;
                }
                else
                {
                    record = EnabledProviders.First().ProvidedValue;
                }
            }
            else
            {
                WorkflowOutput.Message = ActiveCodeProvider().InitMessage;
                return this;
            }

            if (!string.IsNullOrEmpty(record.EngineCodeA) && !string.IsNullOrEmpty(record.EngineCodeB))
            {
                return WorkflowStateFactory.GetMultipleEngineState(WorkflowOutput, record);
            }
            else if (!string.IsNullOrEmpty(record.EngineCodeA) && string.IsNullOrEmpty(record.EngineCodeB))
            {
                return WorkflowStateFactory.GetSingleEngineState(WorkflowOutput, record);
            }

            return this;
        }
    }
}