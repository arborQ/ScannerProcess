using System;

namespace ScannerReader.Models
{
    public class UserModel : BaseObservableModel
    {
        private string _firstName;

        private DateTime? _lastLoginDate;

        private string _lastName;
        public int? Id { get; set; }

        public bool IsModelValid
            => !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(Login);

        public string Login
            =>
                !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName)
                    ? $"{FirstName}.{LastName}".ToLower()
                    : string.Empty;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Login));
                OnPropertyChanged(nameof(IsModelValid));
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Login));
                OnPropertyChanged(nameof(IsModelValid));
            }
        }

        public DateTime? LastLoginDate
        {
            get { return _lastLoginDate; }
            set
            {
                _lastLoginDate = value;
                OnPropertyChanged();
            }
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(Login);
        }
    }
}