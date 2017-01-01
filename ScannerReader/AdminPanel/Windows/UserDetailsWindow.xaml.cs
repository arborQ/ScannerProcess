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
using AdminPanel.Models;

namespace AdminPanel.Windows
{
    /// <summary>
    /// Interaction logic for UserDetailsWindow.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {
        public UserModel User { get; set; }

        public UserDetailsWindow(UserModel userModel)
        {
            Title = userModel == null ? "Create new user" : $"Edit user {userModel.FirstName}, {userModel.LastName}";

            User = userModel ?? new UserModel();
        }

        public UserDetailsWindow() : this(null)
        {
            DataContext = User;
            InitializeComponent();
        }
    }
}
