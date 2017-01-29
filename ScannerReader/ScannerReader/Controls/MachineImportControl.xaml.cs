using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Excel;
using Microsoft.Win32;
using RepositoryServices;
using RepositoryServices.Models;
using ScannerReader.Models;

namespace ScannerReader.Controls
{
    /// <summary>
    ///     Interaction logic for MachineImportControl.xaml
    /// </summary>
    public partial class MachineImportControl
    {
        private readonly ApplicationService _applicationService;
        private readonly ImportViewModel _importViewModel;

        public Action<bool> IsBlocked { get; set; }

        public MachineImportControl(ApplicationService applicationService)
        {
            _importViewModel = new ImportViewModel();
            DataContext = _importViewModel;
            _applicationService = applicationService;
            InitializeComponent();
        }

        private void MachineImportControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            var fileName = AskForExcelFileLocation();
            if (!string.IsNullOrEmpty(fileName))
            {
                _importViewModel.Machines = ImportMachines(fileName).ToList();
            }
            else
            {
                _importViewModel.Machines = null;
            }
        }

        private string AskForExcelFileLocation()
        {
            var dlg = new OpenFileDialog
            {
                DefaultExt = "*.xlsx"
            };

            var result = dlg.ShowDialog();

            if (result.HasValue && result.Value && dlg.FileName.EndsWith(".xls"))
                return dlg.FileName;

            return string.Empty;
        }

        private IEnumerable<Machine> ImportMachines(string excelFilePath)
        {
            using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var excelReader = ExcelReaderFactory.CreateBinaryReader(stream))
                {
                    var records = new List<Machine>();
                    excelReader.Read(); // skip header
                    while (excelReader.Read())
                    {
                        records.Add(new Machine
                        {
                            Code = excelReader.GetString(0),
                            EngineCodeA = excelReader.GetString(1),
                            EngineCodeB = excelReader.GetString(2),
                            EnginePositionA = excelReader.GetInt32(3),
                            EnginePositionB = excelReader.GetInt32(4),
                            ProgramType = excelReader.GetString(5),
                            ImageA = ParseImagePath(excelReader.GetString(6)),
                            ImageB = ParseImagePath(excelReader.GetString(7)),
                            ImageC = ParseImagePath(excelReader.GetString(8)),
                            Comment = excelReader.GetString(9)
                        });
                    }
                    return records;
                }
            }
        }

        private string ParseImagePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            return path.Replace("/", @"\");
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsBlocked != null)
            {
                IsBlocked(true);
            }

            _applicationService.MachineRepository.RemoveRecord(a => a.Id > 0);

            foreach (var machine in _importViewModel.Machines)
            {
                await Task.Factory.StartNew(() =>
                {
                    _applicationService.MachineRepository.AddRecord(machine);
                });
                ProgressBarImport.Value += 1;
                _importViewModel.ProcessedMachines += 1;
            }

            _importViewModel.Machines = null;

            if (IsBlocked != null)
            {
                IsBlocked(false);
            }
        }
    }
}