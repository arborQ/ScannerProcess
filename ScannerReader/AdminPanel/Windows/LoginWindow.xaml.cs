using System.Windows;
using System.Windows.Input;
using AdminPanel.Properties;

namespace AdminPanel.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow
    {
        private readonly bool _shutDownApp;

        public LoginWindow(bool shutDownApp = true)
        {
            _shutDownApp = shutDownApp;
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
                ManualClose();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == Settings.Default.AdminName && PasswordBox.Password == Settings.Default.AdminPassword)
            {
                Hide();
                var userListWindow = new AdminOptionsWindow {Owner = this};
                userListWindow.ShowDialog();
                ManualClose();
            }
            else
            {
                var result = MessageBox.Show(Properties.Resources.WrongLoginOrPasswrodMessage,
                    Properties.Resources.WrongLoginOrPasswrodMessage, MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void ManualClose()
        {
            if (_shutDownApp)
            {
                Application.Current.Shutdown();
            }
            else
            {
                Close();
            }
        }
    }
}