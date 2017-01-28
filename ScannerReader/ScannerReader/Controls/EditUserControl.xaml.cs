using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RepositoryServices;
using RepositoryServices.Models;
using ScannerReader.Models;

namespace ScannerReader.Controls
{
    /// <summary>
    ///     Interaction logic for EditUserControl.xaml
    /// </summary>
    public partial class EditUserControl : UserControl
    {
        private readonly ApplicationService _applicationService;

        public EditUserControl(ApplicationService applicationService, int? userId = null)
        {
            _applicationService = applicationService;
            User = new UserModel {Id = userId};

            DataContext = User;
            InitializeComponent();
        }

        public UserModel User { get; set; }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!User.Id.HasValue)
            {
                _applicationService.UserRepository.AddRecord(new User
                {
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    LastLoginDate = User.LastLoginDate,
                    Login = User.Login
                });

                User.FirstName = string.Empty;
                User.LastName = string.Empty;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (User.Id.HasValue)
            {
                var user = _applicationService.UserRepository.GetRecord(User.Id.Value);
                User.FirstName = user.FirstName;
                User.LastName = user.LastName;
                User.LastLoginDate = user.LastLoginDate;
            }
            else
            {
                User.FirstName = string.Empty;
                User.LastName = string.Empty;
                User.LastLoginDate = null;
            }
        }
    }
}