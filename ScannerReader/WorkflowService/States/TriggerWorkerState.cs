using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RepositoryServices.Models;
using WorkflowService.Interfaces;

namespace WorkflowService.States
{
    public class TriggerWorkerState : WorkflowState
    {
        private readonly Machine _machine;
        private readonly IReadValueService _readValueService;

        public TriggerWorkerState(IWorkflowOutput workflowOutput, Machine machine, IWorkflowStateFactory workflowStateFactory, IReadValueService readValueService)
            : base(workflowOutput, workflowStateFactory)
        {
            _machine = machine;
            _readValueService = readValueService;
        }


        public override string Code => "TRIGGER_WORKER";

        public override IWorkflowState Initialize()
        {
            WorkflowOutput.Message = "Pracuję...";
            //_readValueService.Wait(TimeSpan.FromSeconds(10));

            //var actions = Enumerable.Range(0, TimeSpan.FromSeconds(10).Seconds)
            //    .OrderByDescending(s => s)
            //    .Select(a => new Func<string>(() =>
            //    {
            //        Thread.Sleep(TimeSpan.FromSeconds(1));
            //        return $"Pracuje: {a}s";
            //    }))
            //    .ToList();

            //return WorkflowStateFactory.GetPendingState(WorkflowOutput);

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

            return WorkflowStateFactory.GetPendingState(WorkflowOutput);
        }

        public override bool IsLocked => true;

        public override bool CanBreak => false;

    }
}