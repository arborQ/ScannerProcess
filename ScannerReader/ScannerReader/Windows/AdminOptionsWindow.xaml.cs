using System.Windows;


namespace ScannerReader.Windows
{
    /// <summary>
    /// Interaction logic for AdminOptionsWindow.xaml
    /// </summary>
    public partial class AdminOptionsWindow : Window
    {
        public AdminOptionsWindow()
        {
            InitializeComponent();
        }

        private void ShowUsers_Click(object sender, RoutedEventArgs e)
        {
            var userWindow = new UserList();
            userWindow.ShowDialog();
        }

        private void ShowMachines_Click(object sender, RoutedEventArgs e)
        {
            var userWindow = new MachineListWindow();
            userWindow.ShowDialog();
        }
    }
}
