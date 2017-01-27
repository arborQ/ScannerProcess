using ScannerReader.Controls;
using ScannerReader.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace ScannerReader.Windows
{
    /// <summary>
    /// Interaction logic for AdminOptionsWindow.xaml
    /// </summary>
    public partial class AdminOptionsWindow : Window
    {
        private IControlFactory _controlFactory;
        public AdminOptionsWindow(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            InitializeComponent();
        }
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            machineStackPanel.Children.Add(_controlFactory.CreateUserListControl());
            usersStackPanel.Children.Add(_controlFactory.CreateUserListControl());
            addUsersStackPanel.Children.Add(_controlFactory.CreateEditUserControl(null));
        }
    }
}
