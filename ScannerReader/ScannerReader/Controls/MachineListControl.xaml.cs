using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using RepositoryServices;
using RepositoryServices.Models;
using ScannerReader.Models;

namespace ScannerReader.Controls
{
    /// <summary>
    ///     Interaction logic for MachineListControl.xaml
    /// </summary>
    public partial class MachineListControl
    {
        private readonly ApplicationService _applicationService;
        private readonly MachineListViewModel _machieListViewModel;

        public MachineListControl(ApplicationService applicationService)
        {
            _machieListViewModel = new MachineListViewModel();
            DataContext = _machieListViewModel;

            _applicationService = applicationService;
            InitializeComponent();
        }

        private void MachineListControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            _machieListViewModel.Machines = _applicationService.MachineRepository.GetRecords().Take(200).ToList();
        }
    }
}