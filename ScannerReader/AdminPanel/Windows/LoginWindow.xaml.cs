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
using AdminPanel.Models;
using AdminPanel.Properties;

namespace AdminPanel.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginModel Model { get; set; }

        public LoginWindow()
        {
            Model = new LoginModel
            {
                Login = "admin"
            };

            DataContext = Model;
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
            if (LoginBox.Text == Settings.Default.AdminName && PasswordBox.Password == Settings.Default.AdminPassword)
            {
                Hide();
                var userListWindow = new UserList {Owner = this};
                userListWindow.ShowDialog();
                Application.Current.Shutdown();
            }
            else
            {
                var result = MessageBox.Show(Properties.Resources.WrongLoginOrPasswrodMessage, Properties.Resources.WrongLoginOrPasswrodMessage, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
    }
}
