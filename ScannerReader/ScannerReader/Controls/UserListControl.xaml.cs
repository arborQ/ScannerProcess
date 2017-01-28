using RepositoryServices;
using ScannerReader.Models;
using ScannerReader.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScannerReader.Controls
{
    /// <summary>
    /// Interaction logic for UsersControl.xaml
    /// </summary>
    public partial class UserListControl : UserControl, INotifyPropertyChanged
    {
        public Action<UserModel> OnUserEdit { get; set; }
                
        public UserListViewModel UserListViewModel { get; set; }

        private ApplicationService _applicationService;

        public event PropertyChangedEventHandler PropertyChanged;

        public UserListControl(ApplicationService applicationService)
        {
            UserListViewModel = new UserListViewModel();
            _applicationService = applicationService;
            InitializeComponent();
            DataContext = UserListViewModel;
        }

        private void OpenUserEdit(object sender, MouseButtonEventArgs e)
        {
            if(OnUserEdit == null)
            {
                return;
            }

            var selectedUser = ((ListViewItem)sender).Content as UserModel;

            if (selectedUser == null)
            {
                throw new Exception("this should not happened");
            }

            OnUserEdit(selectedUser);
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
                _applicationService.UserRepository.RemoveRecord(selectedUser.Id.Value);
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

        private void OpenCreateNewUserWidnow(object sender, RoutedEventArgs e)
        {

        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            UserListViewModel.List = _applicationService
                .UserRepository
                .GetRecords()
                .Select(u => new UserModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    LastLoginDate = u.LastLoginDate
                })
                .ToList();
        }
    }
}
