using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;
using RepositoryServices;
using RepositoryServices.Models;
using Excel;

namespace ScannerReader.Windows
{
    /// <summary>
    /// Interaction logic for MachineListWindow.xaml
    /// </summary>
    public partial class MachineListWindow : INotifyPropertyChanged
    {
        private ApplicationService Service { get; }
        public List<Machine> Items { get; set; }

        public MachineListWindow()
        {
            Service = new ApplicationService();
            Items = new List<Machine>();
            InitializeComponent();
            DataContext = this;
        }

        private void ImportMachineCodes(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                DefaultExt = "*.xlsx",
            };

            var result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                try
                {
                    using (var stream = File.Open(dlg.FileName, FileMode.Open, FileAccess.Read))
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
                                    ImageA = excelReader.GetString(6),
                                    ImageB = excelReader.GetString(7),
                                    ImageC = excelReader.GetString(8),
                                    Comment = excelReader.GetString(9)
                                });
                            }

                            Service.MachineRepository.RemoveRecord(r => r.Id > 0);
                            foreach (var machine in records)
                            {
                                Service.MachineRepository.AddRecord(machine);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    WindowLoaded();
                }
            }
        }

        private void WindowLoaded(object sender = null, RoutedEventArgs e = null)
        {
            var items = Service.MachineRepository.GetRecords().OrderBy(r => r.Code).ToList();
            Items = items;
            OnPropertyChanged(nameof(Items));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}