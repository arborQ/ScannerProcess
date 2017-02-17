using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ScannerReader.Models;

namespace ScannerReader.Windows
{
    /// <summary>
    ///     Interaction logic for GetValueWindow.xaml
    /// </summary>
    public partial class GetValueWindow
    {
        private readonly string _expectedValue;
        public GetValueViewModel Model;

        public GetValueWindow(string expectedValue)
        {
            _expectedValue = expectedValue;
            Model = new GetValueViewModel { Value = string.Empty };
            DataContext = Model;

            InitializeComponent();
        }

        private void ValueBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void GetValueWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //var screenWidth = SystemParameters.PrimaryScreenWidth;
            //var screenHeight = SystemParameters.PrimaryScreenHeight;
            //var windowWidth = Width;
            //var windowHeight = Height;
            //Left = screenWidth / 2 - windowWidth / 2;
            //Top = screenHeight / 2 - windowHeight / 2;
            Left = 0;
            Top = 200;
        }

        private void ValueBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (Model.Value == _expectedValue)
            {
                Close();
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Model.Value = string.Empty;
            Close();
        }
    }
}