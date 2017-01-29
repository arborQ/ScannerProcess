namespace ScannerReader.Models
{
    public class AdminOptionsViewModel : BaseObservableModel
    {
        private UserModel _editedUser;
        private bool _isMachineImport;

        public bool IsUserEditing => EditedUser != null;

        public bool IsMachineImport
        {
            get { return _isMachineImport; }
            set
            {
                _isMachineImport = value;
                OnPropertyChanged();
            }
        }

        public UserModel EditedUser
        {
            get { return _editedUser; }

            set
            {
                _editedUser = value;
                OnPropertyChanged(nameof(IsUserEditing));
            }
        }
    }
}