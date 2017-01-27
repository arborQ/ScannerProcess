using RepositoryServices;
using ScannerReader.Models;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for EditUserControl.xaml
    /// </summary>
    public partial class EditUserControl : UserControl
    {
        public UserModel User { get; set; }
        private ApplicationService _applicationService;

        public EditUserControl(ApplicationService applicationService, int? userId = null)
        {
            _applicationService = applicationService;
            User = new UserModel { Id = userId };

            DataContext = User;
            InitializeComponent();
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!User.Id.HasValue)
            {
                _applicationService.UserRepository.AddRecord(new RepositoryServices.Models.User
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
