using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Common.Interfaces;
using Logger.Interfaces;
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
        private readonly ILogService _logger;
        private readonly ApplicationService _applicationService;
        private readonly IUserSecurity _userSecurity;

        public LoginWindow(IWindowFactory windowFactory, ILogService logger, ApplicationService applicationService, IUserSecurity userSecurity)
        {
            _windowFactory = windowFactory;
            _logger = logger;
            _applicationService = applicationService;
            _userSecurity = userSecurity;
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
                if (LoginBox.Text == "admin" && PasswordBox.Password == "admin")
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

            var isSuccesfullLogin = _userSecurity.ValidateUser(LoginBox.Text, PasswordBox.Password);

            if (!isSuccesfullLogin)
            {
                MessageBox.Show(string.Format(Properties.Resources.InvalidUserLoginMessageFormat, LoginBox.Text),
                    string.Empty,
                    MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else
            {
                SuccessfullLogin(LoginBox.Text);
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
    }
}