using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RepositoryServices;
using ScannerReader.Models;

namespace ScannerReader.Controls
{
    /// <summary>
    ///     Interaction logic for UsersControl.xaml
    /// </summary>
    public partial class UserListControl
    {
        private readonly ApplicationService _applicationService;

        public UserListControl(ApplicationService applicationService)
        {
            UserListViewModel = new UserListViewModel();
            _applicationService = applicationService;
            InitializeComponent();
            DataContext = UserListViewModel;
        }

        public Action<UserModel> OnUserEdit { get; set; }

        public UserListViewModel UserListViewModel { get; set; }

        private void OpenUserEdit(object sender, MouseButtonEventArgs e)
        {
            if (OnUserEdit == null)
                return;

            var selectedUser = ((ListViewItem) sender).Content as UserModel;

            if (selectedUser == null)
                throw new Exception("this should not happened");

            OnUserEdit(selectedUser);
        }

        private void RemoveUser(object sender)
        {
            var selectedUser = ((ListViewItem) sender).Content as UserModel;
            if (selectedUser == null)
                throw new Exception("this should not happened");
            var removeMessage = string.Format(Properties.Resources.RemoveUserConfirmFormat,
                $"{selectedUser.FirstName}, {selectedUser.LastName}");

            var confirmResult = MessageBox.Show(removeMessage, removeMessage, MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmResult == MessageBoxResult.Yes)
            {
                _applicationService.UserRepository.RemoveRecord(selectedUser.Id.Value);
                WindowLoaded(null, null);
            }
        }

        private void OpenUserEditKeyboard(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                OpenUserEdit(sender, null);
            else if (e.Key == Key.Delete)
                RemoveUser(sender);
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