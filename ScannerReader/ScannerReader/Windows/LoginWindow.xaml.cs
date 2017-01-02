using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using RepositoryServices;

namespace ScannerReader.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow
    {
        public LoginWindow()
        {
            Service = new ApplicationService();
            InitializeComponent();
        }

        private ApplicationService Service { get; }

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
            if (LoginBox.Text == "admin" && string.IsNullOrEmpty(PasswordBox.Password))
            {
                Hide();
                var adminLogin = new AdminPanel.Windows.LoginWindow(false);
                adminLogin.ShowDialog();
                Show();
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

            var user = Service.UserRepository.GetRecords(u => u.Login == LoginBox.Text).SingleOrDefault();

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
                Service.UserRepository.EditRecord(u => u.Id == user.Id, u =>
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
                    Service.UserRepository.EditRecord(u => u.Id == user.Id, u => { u.LastLoginDate = DateTime.Now; });
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
            Hide();
            var userListWindow = new WorkflowWindow(userLogin) {Owner = this};
            userListWindow.ShowDialog();
            Application.Current.Shutdown();
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