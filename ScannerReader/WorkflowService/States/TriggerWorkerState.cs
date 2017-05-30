using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RepositoryServices.Models;
using WorkflowService.Interfaces;
using RepositoryServices;
using System.IO;
using ControllerService;
using Logger.Interfaces;
using CrossCutting;

namespace WorkflowService.States
{
    public class TriggerWorkerState : WorkflowState
    {
        private readonly Machine _machine;
        private readonly ApplicationService _applicationService;
        private readonly IReadValueService _readValueService;
        private readonly IControllerServiceFactory _controllerServiceFactory;
        private readonly ILogService _logService;
        private readonly IUserSecurity _userSecurity;

        public TriggerWorkerState(IWorkflowOutput workflowOutput,
            Machine machine,
            ILogService logService,
            IUserSecurity userSecurity,
            IWorkflowStateFactory workflowStateFactory,
            ApplicationService applicationService,
            IControllerServiceFactory controllerServiceFactory,
            IReadValueService readValueService)
            : base(workflowOutput, workflowStateFactory)
        {
            _logService = logService;
            _machine = machine;
            _applicationService = applicationService;
            _readValueService = readValueService;
            _controllerServiceFactory = controllerServiceFactory;
            _userSecurity = userSecurity;
        }

        private string _baseDirectory;
        private string BaseDirectory => _baseDirectory ?? (_baseDirectory = _applicationService.SettingsRepository.Get().ImagePath);

        public override string Code => "TRIGGER_WORKER";

        public override IWorkflowState Initialize()
        {
            WorkflowOutput.Message = "Łączenie ze sterownikiem...";

            return null;
        }

        public override async Task<IWorkflowState> AsyncInitialize()
        {
            var settings = _applicationService.SettingsRepository.Get();

            if (settings.DrilEnabled == 1)
            {
                var service = _controllerServiceFactory.Create(new ControllerEvents
                {
                    ChangeState = message => WorkflowOutput.Message = message,
                    Error = exception => WorkflowOutput.Message = exception.Message,
                }, settings.IpAddress);

                int jobId = 0;
                if (!int.TryParse(_machine.ProgramType, out jobId))
                {
                    WorkflowOutput.Message = $"'{_machine.ProgramType}' nie jest poprawnym numerem zadania";
                    return this;
                }

                if (!await service.SelectJobAsync(jobId))
                {
                    WorkflowOutput.Message = "Nie udało się uruchomić zadania";
                    await Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(5000);
                    });
                }
                else
                {
                    WorkflowOutput.ImagePath = GetImagePath(_machine.ImageC);
                }
            }
            else
            {
                WorkflowOutput.ImagePath = GetImagePath(_machine.ImageC);
            }

            _logService.LogScaningDone(_userSecurity.CurrentUserLogin(), _machine.Code, _machine.EngineCodeA, _machine.EngineCodeB, _machine.EnginePositionA, _machine.EnginePositionB, _machine.ProgramType);

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