using System.Windows;

namespace AdminPanel.Windows
{
    /// <summary>
    /// Interaction logic for UserList.xaml
    /// </summary>
    public partial class UserList : Window
    {
        public UserList()
        {
            InitializeComponent();
        }

        private void OpenCreateNewUserWidnow(object sender, RoutedEventArgs e)
        {
            var newUserWindow = new UserDetailsWindow();
            newUserWindow.ShowDialog();
        }
    }
}