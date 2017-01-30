using WorkflowService;

namespace ScannerReader.Models
{
    public class WorkflowOutput : BaseObservableModel, IWorkflowOutput
    {
        private string _description;
        private string _imagePath;
        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
    }
}