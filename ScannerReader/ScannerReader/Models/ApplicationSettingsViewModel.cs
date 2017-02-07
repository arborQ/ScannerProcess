namespace ScannerReader.Models
{
    public class ApplicationSettingsViewModel : BaseObservableModel
    {
        private string _imagePath;
        private int _activityTimeout;

        public int ActivityTimeout
        {
            get { return _activityTimeout; }
            set
            {
                _activityTimeout = value;
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
    }
}