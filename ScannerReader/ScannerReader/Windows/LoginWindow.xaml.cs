using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Common.Interfaces;
using RepositoryServices;
using ScannerReader.Interfaces;

namespace ScannerReader.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow
    {
        private readonly IWindowFactory _windowFactory;
        private readonly ApplicationService _applicationService;

        public LoginWindow(IWindowFactory windowFactory, ApplicationService applicationService)
        {
            _windowFactory = windowFactory;
            _applicationService = applicationService;
            InitializeComponent();
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Button_Click(sender, e);
            }
            else if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
#if DEBUG
            if (LoginBox.Text == "admin" && PasswordBox.Password == "admin")
#endif

#if !DEBUG
                if (LoginBox.Text == "arbor" && PasswordBox.Password == "software")
#endif
            {
                LoginBox.Clear();
                PasswordBox.Clear();

                Visibility = Visibility.Hidden;
                _windowFactory.CreateAdminOptionsWindow().ShowDialog();
                Visibility = Visibility.Visible;
                return;
            }

            if (string.IsNullOrEmpty(LoginBox.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show(Properties.Resources.InvalidModelLoginMessage, string.Empty,
                    MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            var passwordLenght = 5;
            if (PasswordBox.Password.Length < passwordLenght)
            {
                MessageBox.Show(string.Format(Properties.Resources.PasswordToShortMessage, passwordLenght), string.Empty,
                    MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            var user = _applicationService.UserRepository.GetRecords(u => u.Login == LoginBox.Text).SingleOrDefault();

            if (user == null)
            {
                MessageBox.Show(string.Format(Properties.Resources.InvalidUserLoginMessageFormat, LoginBox.Text),
                    string.Empty,
                    MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            var passwordHash = HashPassword(PasswordBox.Password);
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                _applicationService.UserRepository.EditRecord(u => u.Id == user.Id, u =>
                {
                    u.PasswordHash = passwordHash;
                    u.LastLoginDate = DateTime.Now;
                });
                SuccessfullLogin(LoginBox.Text);
            }
            else
            {
                if (user.PasswordHash == passwordHash)
                {
                    _applicationService.UserRepository.EditRecord(u => u.Id == user.Id, u => { u.LastLoginDate = DateTime.Now; });
                    SuccessfullLogin(LoginBox.Text);
                }
                else
                {
                    MessageBox.Show(string.Format(Properties.Resources.InvalidUserLoginMessageFormat, LoginBox.Text),
                        string.Empty, MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void SuccessfullLogin(string userLogin)
        {
            LoginBox.Clear();
            PasswordBox.Clear();
            Hide();
            var userSecurity = Bootstrapper.Resolve<IUserSecurity>();
            userSecurity.SetCurrentUser(userLogin);
            var userListWindow = _windowFactory.CreateWorkflowWindow();
            userListWindow.ShowDialog();
            Show();
            userSecurity.SetCurrentUser(null);
            LoginBox.Focus();
        }

        private string HashPassword(string password)
        {
            var data = Encoding.ASCII.GetBytes(password);
            var md5 = new MD5CryptoServiceProvider();
            var md5Data = md5.ComputeHash(data);
            return Encoding.ASCII.GetString(md5Data);
        }
    }
}