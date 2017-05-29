using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ScannerReader.Models;

namespace ScannerReader.Controls
{
    /// <summary>
    ///     Interaction logic for ApplicationSettingsControl.xaml
    /// </summary>
    public partial class ApplicationSettingsControl
    {
        private readonly ApplicationSettingsViewModel _model = new ApplicationSettingsViewModel();

        public ApplicationSettingsControl()
        {
            DataContext = _model;
            InitializeComponent();
        }

        public Func<ApplicationSettingsViewModel> LoadApplicationSettings { get; set; }
        public Func<ApplicationSettingsViewModel, bool> SaveData { get; set; }


        private void ApplicationSettingsControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = LoadApplicationSettings?.Invoke();

            if (model != null)
            {
                _model.ImagePath = model.ImagePath;
                _model.ActivityTimeout = model.ActivityTimeout;
                _model.DrilEnabled = model.DrilEnabled;
                _model.SelectedMode = model.SelectedMode;
                _model.IpAddress = model.IpAddress ?? "192.168.1.103";
            }
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var openDialog = new Gat.Controls.OpenDialogView();

            var vm = (Gat.Controls.OpenDialogViewModel)openDialog.DataContext;

            vm.IsDirectoryChooser = true;
            vm.Show();

            _model.ImagePath = vm.SelectedFolder?.Path;
        }

        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveData?.Invoke(_model);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _model.ImagePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? string.Empty;
            _model.ActivityTimeout = 10;
            _model.DrilEnabled = OptionEnabled.Yes;
            _model.SelectedMode = ModeOptions.OrderNumber;
            _model.IpAddress = "192.168.1.103";

            SaveData?.Invoke(_model);
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}