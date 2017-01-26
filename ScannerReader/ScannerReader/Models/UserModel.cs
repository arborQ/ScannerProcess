using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScannerReader.Models
{
    public class UserModel : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _firstName;

        private string _lastName;

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
            }
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(Login);
        }

        private DateTime? _lastLoginDate;

        public DateTime? LastLoginDate
        {
            get { return _lastLoginDate; }
            set { _lastLoginDate = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}