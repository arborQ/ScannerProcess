using System;
using System.Linq;

namespace WorkflowService
{
    public class Workflow
    {
        private string CurrentStateCode { get; set; }

        public Workflow(string startCode = "PENDING")
        {
            CurrentStateCode = startCode;
        }

        public void Trigger(string input)
        {
            DisplayMessage?.Invoke($"New message {input}");
        }

        public Action<string> DisplayMessage;
        public Action<string> UpdateDescription;
        public Action<string> UpdateImage;
    }

    public class WorkflowItem
    {
        public string MachineNumber { get; set; }
    }

    public class WorkflowState
    {
        public string Code => "PENDING";

        private WorkflowAction[] Actions => new [] { new WorkflowAction {} };

        public WorkflowAction Trigger(string input)
        {
            var action = Actions.FirstOrDefault(wa => wa.CanHandleInput(input));
            if (action == null)
            {

            }
            else
            {
                
            }
            return null;
        }
    }

    public class WorkflowAction
    {
        public bool CanHandleInput(string input)
        {
            return true;
        }

    }
}