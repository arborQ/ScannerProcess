using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RepositoryServices;
using ScannerReader.Models;

namespace ScannerReader.Windows
{
    /// <summary>
    /// Interaction logic for UserList.xaml
    /// </summary>
    public partial class UserList : Window, INotifyPropertyChanged
    {
        public UserList()
        {
            Service = new ApplicationService();
            DataContext = this;
            InitializeComponent();
        }

        private ApplicationService Service { get; }
        public List<UserModel> List { get; set; }

        private void OpenCreateNewUserWidnow(object sender, RoutedEventArgs e)
        {
            var newUserWindow = new UserDetailsWindow(null);
            newUserWindow.ShowDialog();
            WindowLoaded(sender, e);
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var items = Service.UserRepository.GetRecords().ToList();
            List = items.Select(u => new UserModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                LastLoginDate = u.LastLoginDate
            }).ToList();

            OnPropertyChanged(nameof(List));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OpenUserEdit(object sender, MouseButtonEventArgs e)
        {
            var selectedUser = ((ListViewItem)sender).Content as UserModel;
            if (selectedUser == null)
            {
                throw new Exception("this should not happened");
            }
            var newUserWindow = new UserDetailsWindow(selectedUser.Id);
            newUserWindow.ShowDialog();
            WindowLoaded(sender, e);
        }

        private void RemoveUser(object sender)
        {
            var selectedUser = ((ListViewItem)sender).Content as UserModel;
            if (selectedUser == null)
            {
                throw new Exception("this should not happened");
            }
            var removeMessage = string.Format(Properties.Resources.RemoveUserConfirmFormat,
                $"{selectedUser.FirstName}, {selectedUser.LastName}");

            var confirmResult = MessageBox.Show(removeMessage, removeMessage, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmResult == MessageBoxResult.Yes)
            {
                Service.UserRepository.RemoveRecord(selectedUser.Id.Value);
                WindowLoaded(null, null);
            }
        }

        private void OpenUserEditKeyboard(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                OpenUserEdit(sender, null);
            }
            else if (e.Key == Key.Delete)
            {
                RemoveUser(sender);
            }
        }
    }
}