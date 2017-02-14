using System.Windows;
using RepositoryServices;
using RepositoryServices.Models;
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
        private readonly ApplicationService _applicationService;

        public AdminOptionsViewModel Model;

        public AdminOptionsWindow(IControlFactory controlFactory, ApplicationService applicationService)
        {
            _controlFactory = controlFactory;
            _applicationService = applicationService;
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

            MachineStackPanel.Children.Add(_controlFactory.CreateMachineListControl());
            UsersStackPanel.Children.Add(userList);
            AddUsersStackPanel.Children.Add(_controlFactory.CreateEditUserControl(null));
            var import = _controlFactory.CreateMachineImportControl();
            import.IsBlocked = isblocked =>
            {
                Model.IsMachineImport = isblocked;
            };
            MachineImportStackPanel.Children.Add(import);
        }

        private bool UpdateSettings(ApplicationSettingsViewModel model)
        {
            _applicationService.SettingsRepository.Update(new Settings
            {
                ImagePath = model.ImagePath,
                DefaultTimeout = model.ActivityTimeout
            });

            return true;
        }

        private ApplicationSettingsViewModel LoadSettings()
        {
            var settings = _applicationService.SettingsRepository.Get();

            return new ApplicationSettingsViewModel
            {
                ImagePath = settings.ImagePath,
                ActivityTimeout = settings.DefaultTimeout
            };
        }
    }
}