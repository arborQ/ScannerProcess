using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RepositoryServices.Models;
using WorkflowService.Interfaces;
using RepositoryServices;
using System.IO;

namespace WorkflowService.States
{
    public class TriggerWorkerState : WorkflowState
    {
        private readonly Machine _machine;
        private readonly ApplicationService _applicationService;
        private readonly IReadValueService _readValueService;

        public TriggerWorkerState(IWorkflowOutput workflowOutput, 
            Machine machine, 
            IWorkflowStateFactory workflowStateFactory, 
            ApplicationService applicationService,
            IReadValueService readValueService)
            : base(workflowOutput, workflowStateFactory)
        {
            _machine = machine;
            _applicationService = applicationService;
            _readValueService = readValueService;
        }

        private string _baseDirectory;
        private string BaseDirectory => _baseDirectory ?? (_baseDirectory = _applicationService.SettingsRepository.Get().ImagePath);

        public override string Code => "TRIGGER_WORKER";

        public override IWorkflowState Initialize()
        {
            WorkflowOutput.Message = "Pracuję...";

            return null;
        }

        public override async Task<IWorkflowState> AsyncInitialize()
        {
            var actions = Enumerable.Range(0, TimeSpan.FromSeconds(10).Seconds)
                .OrderByDescending(s => s)
                .Select(a => new Func<string>(() =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return $"Pracuje: {a}s";
                }))
                .ToList();

            foreach (var action in actions)
            {
                var message = await Task.Factory.StartNew(action);
                WorkflowOutput.Message = message;
            }

            WorkflowOutput.ImagePath = GetImagePath(_machine.ImageC);

            return WorkflowStateFactory.GetPendingState(WorkflowOutput);
        }

        public override bool IsLocked => true;

        public override bool CanBreak => false;

        private string GetImagePath(string path)
        {
            return string.IsNullOrEmpty(path) ? null : Path.Combine(BaseDirectory, path);
        }

    }
}