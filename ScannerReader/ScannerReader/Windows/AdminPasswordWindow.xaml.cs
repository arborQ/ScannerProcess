using System.Windows;
using System.Windows.Input;

namespace ScannerReader.Windows
{
    /// <summary>
    ///     Interaction logic for AdminPasswordWindow.xaml
    /// </summary>
    public partial class AdminPasswordWindow
    {
        public AdminPasswordWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void PasswordBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                ButtonBase_OnClick(sender, e);
        }
    }
}