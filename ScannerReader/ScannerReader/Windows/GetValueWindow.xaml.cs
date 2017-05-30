using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ScannerReader.Models;
using WorkflowService.Interfaces;

namespace ScannerReader.Windows
{
    /// <summary>
    ///     Interaction logic for GetValueWindow.xaml
    /// </summary>
    public partial class GetValueWindow
    {
        private readonly string _expectedValue;
        public GetValueViewModel Model;
        private readonly DialogPosition _position;
        public GetValueWindow(string expectedValue, DialogPosition position = DialogPosition.Center)
        {
            _expectedValue = expectedValue;
            Model = new GetValueViewModel { Value = string.Empty };
            DataContext = Model;
            _position = position;
            InitializeComponent();
        }

        private void ValueBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void GetValueWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;
            var windowWidth = Width;
            var windowHeight = Height;

            Top = screenHeight - windowHeight - 100;

            switch (_position)
            {
                case DialogPosition.Center:
                    Left = screenWidth / 2 - windowWidth / 2;
                    return;
                case DialogPosition.Left:
                    Left = 200;
                    return;
                case DialogPosition.Right:
                    Left = screenWidth - windowWidth - 200;
                    return;
                default:
                    Left = 0;
                    Top = 200;
                    return;
            }            
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