namespace ScannerReader.Models
{
    public class GetValueViewModel : BaseObservableModel
    {
        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }
    }
}