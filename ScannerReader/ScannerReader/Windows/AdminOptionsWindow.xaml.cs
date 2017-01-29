using System.Windows;
using ScannerReader.Interfaces;
using ScannerReader.Models;

namespace ScannerReader.Windows
{
    /// <summary>
    ///     Interaction logic for AdminOptionsWindow.xaml
    /// </summary>
    public partial class AdminOptionsWindow : Window
    {
        private readonly IControlFactory _controlFactory;

        public AdminOptionsViewModel Model;

        public AdminOptionsWindow(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            DataContext = Model = new AdminOptionsViewModel();
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var userList = _controlFactory.CreateUserListControl();
            userList.OnUserEdit = u =>
            {
                Model.EditedUser = u;

                var editComponent = _controlFactory.CreateEditUserControl(u.Id);
                editComponent.OnSaved = () =>
                {
                    Model.EditedUser = null;
                };

                EditUsersStackPanel.Children.Clear();
                EditUsersStackPanel.Children.Add(editComponent);
            };

            machineStackPanel.Children.Add(_controlFactory.CreateUserListControl());
            UsersStackPanel.Children.Add(userList);
            AddUsersStackPanel.Children.Add(_controlFactory.CreateEditUserControl(null));
        }
    }
}