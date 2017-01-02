using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdminPanel.Windows
{
    /// <summary>
    /// Interaction logic for AdminOptionsWindow.xaml
    /// </summary>
    public partial class AdminOptionsWindow : Window
    {
        public AdminOptionsWindow()
        {
            InitializeComponent();
        }

        private void ShowUsers_Click(object sender, RoutedEventArgs e)
        {
            var userWindow = new UserList();
            userWindow.ShowDialog();
        }

        private void ShowMachines_Click(object sender, RoutedEventArgs e)
        {
            var userWindow = new MachineListWindow();
            userWindow.ShowDialog();
        }
    }
}
