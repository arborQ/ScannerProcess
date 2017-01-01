using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using RepositoryServices;
using RepositoryServices.Models;

namespace ScannerReader.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private ApplicationService Service { get; }

        public LoginWindow()
        {
            Service = new ApplicationService();
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
            if (string.IsNullOrEmpty(LoginBox.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show(Properties.Resources.InvalidModelLoginMessage, string.Empty,
                    MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            var user = Service.UserRepository.GetRecords(u => u.Login == LoginBox.Text).SingleOrDefault();

            if (user == null)
            {
                MessageBox.Show(string.Format(Properties.Resources.InvalidUserLoginMessageFormat, LoginBox.Text), string.Empty,
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
                    Service.UserRepository.EditRecord(u => u.Id == user.Id, u =>
                    {
                        u.LastLoginDate = DateTime.Now;
                    });
                    SuccessfullLogin(LoginBox.Text);
                }
                else
                {
                    MessageBox.Show(string.Format(Properties.Resources.InvalidUserLoginMessageFormat, LoginBox.Text), string.Empty, MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }
            }
        }

        private void SuccessfullLogin(string userLogin)
        {
            Hide();
            var userListWindow = new WorkflowWindow { Owner = this };
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
