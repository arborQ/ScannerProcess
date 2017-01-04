using System.ComponentModel;
using System.Runtime.CompilerServices;
using AdminPanel.Annotations;
using WorkflowService;

namespace ScannerReader.Models
{
    public class WorkflowOutput : IWorkflowOutput, INotifyPropertyChanged
    {
        private string _description;
        private string _imagePath;
        private string _message;

        public event PropertyChangedEventHandler PropertyChanged;

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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}