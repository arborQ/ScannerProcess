namespace WorkflowService
{
    public interface IWorkflowOutput
    {
        string Message { set; }

        string ImagePath { set; }

        string Description { set; }
    }
}