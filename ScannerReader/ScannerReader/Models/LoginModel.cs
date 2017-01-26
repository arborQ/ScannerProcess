using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScannerReader.Models
{
    public class LoginModel : INotifyPropertyChanged
    {
        public string Login { get; set; }

        public string Password { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}