using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdminPanel.Models
{
    public class UserModel : INotifyPropertyChanged
    {
        public long Id { get; set; }

        public string Login => !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) ? $"{FirstName}.{LastName}".ToLower(): string.Empty;

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        public DateTime? LastLoginDate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}