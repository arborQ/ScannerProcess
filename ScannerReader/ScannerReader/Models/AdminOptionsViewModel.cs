namespace ScannerReader.Models
{
    public class AdminOptionsViewModel : BaseObservableModel
    {
        private UserModel _editedUser;
        public bool IsUserEditing => EditedUser != null;

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