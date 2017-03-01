using System.Collections.ObjectModel;
using WorkflowService;

namespace ScannerReader.Models
{
    public class WorkflowOutput : BaseObservableModel, IWorkflowOutput
    {
        private string _description;
        private string _imagePath;
        private string _message;
        private MessageType _messageType;

        public MessageType MessageType
        {
            get
            {
                return _messageType;
            }

            set
            {
                _messageType = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                MessageType = MessageType.Message;
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

        public ObservableCollection<string> Actions { get; set; } = new ObservableCollection<string>();
    }
}