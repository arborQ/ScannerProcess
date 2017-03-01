namespace WorkflowService
{
    public interface IWorkflowOutput
    {
        string Message { set; }

        string ImagePath { set; }

        string Description { set; }

        MessageType MessageType { set; }
    }

    public enum MessageType
    {
        Message = 0,
        Warning = 1,
        Error = 2
    }
}