using System.Collections.Generic;

namespace ScannerReader.Models
{
    public class UserListViewModel : BaseObservableModel
    {
        private List<UserModel> _list;
        public List<UserModel> List
        {
            get
            {
                return _list;
            }

            set
            {
                _list = value;
                OnPropertyChanged();
            }
        }
    }
}
