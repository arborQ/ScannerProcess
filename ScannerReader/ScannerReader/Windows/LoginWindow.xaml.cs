using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Common.Interfaces;
using Logger.Interfaces;
using RepositoryServices;
using ScannerReader.Interfaces;
using CodeGenerator;

namespace ScannerReader.Windows
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow
    {
        private readonly ApplicationService _applicationService;
        private readonly ILogService _logger;
        private readonly IUserSecurity _userSecurity;
        private readonly IWindowFactory _windowFactory;
        private readonly IKeyboardReader _keyboardReader;
        private readonly ICodeSerializer _codeSerializer;
        private const string AdminPassword = "admin";

        public LoginWindow(IWindowFactory windowFactory, ILogService logger, ApplicationService applicationService,
            IUserSecurity userSecurity, IKeyboardReader keyboardReader, ICodeSerializer codeSerializer)
        {
            _windowFactory = windowFactory;
            _logger = logger;
            _applicationService = applicationService;
            _userSecurity = userSecurity;
            _keyboardReader = keyboardReader;
            _codeSerializer = codeSerializer;

            InitializeComponent();
        }


        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                Button_Click(sender, e);
            else if (e.Key == Key.Escape)
                Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
#if DEBUG
            if (LoginBox.Text == "admin" && PasswordBox.Password == AdminPassword)
#endif

#if !DEBUG
                if (LoginBox.Text == "admin" && PasswordBox.Password == AdminPassword)
#endif
            {
                SuccessfullAdminLogin();
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
                MessageBox.Show(string.Format(Properties.Resources.InvalidUserLoginMessageFormat, LoginBox.Text),
                    string.Empty,
                    MessageBoxButton.OK, MessageBoxImage.Stop);
            else
                SuccessfullLogin(LoginBox.Text);
        }

        private void SuccessfullLogin(string userLogin)
        {
            LoginBox.Clear();
            PasswordBox.Clear();
            Hide();

            _userSecurity.SetCurrentUser(userLogin);
            var userListWindow = _windowFactory.CreateWorkflowWindow();
            userListWindow.ShowDialog();
            Show();
            _logger.LogOut(_userSecurity.CurrentUserLogin(), !string.IsNullOrEmpty(userListWindow.CloseReason));
            _userSecurity.SetCurrentUser(null);
            LoginBox.Focus();
        }

        private void SuccessfullAdminLogin()
        {
            LoginBox.Clear();
            PasswordBox.Clear();

            Visibility = Visibility.Hidden;
            _windowFactory.CreateAdminOptionsWindow().ShowDialog();
            Visibility = Visibility.Visible;
            return;
        }


        private void LoginWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _keyboardReader.Dispose();

#if !DEBUG
            var adminPassword = _windowFactory.AdminPasswordWindow();
            var result = adminPassword.ShowDialog();

            if (!(result.HasValue && result.Value && adminPassword.PasswordBox.Password == AdminPassword))
            {
                e.Cancel = true;
            }

            _logger.AdminFail();
#endif

            _keyboardReader.Dispose();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var readerResonse = string.Empty;
            switch (e.Key)
            {
#if DEBUG
                case Key.F1:
                    readerResonse = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJsb2dpbiI6ImFkbWluIn0.GMjXyR82NuhA37Vqf5Mu9r9Riq_LtQBcKd-HZAHaEKU";
                    break;
                case Key.F2:
                    readerResonse = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJsb2dpbiI6IsWCdWthc3oud8OzamNpayJ9.A7Olmo5D12-lT9YVvmg3zize9HIXSah_adPts4EXlbY";
                    break;
#endif
                default:
                    readerResonse = _keyboardReader.NotifyChar(e.Key);
                    break;
            }

            if (!string.IsNullOrEmpty(readerResonse))
            {
                var login = _codeSerializer.DeserializeCode(readerResonse);

                if (string.IsNullOrEmpty(login))
                {
                    _logger.InvalidLogin("BarCodeLoginFail");
                    return;
                }

                if (login == "admin")
                {
                    SuccessfullAdminLogin();
                    return;
                }

                if (!_userSecurity.ValidateBarCodeUser(login))
                {
                    _logger.InvalidLogin("BarCodeLoginFail: " + login);
                    return;
                }

                _logger.SuccesfulLogin("BarCodeLoginSuccess: " + login);
                SuccessfullLogin(login);
            }
        }
    }
}