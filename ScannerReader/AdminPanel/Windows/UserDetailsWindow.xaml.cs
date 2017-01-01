using System;
using System.Windows;
using System.Windows.Input;
using AdminPanel.Models;
using RepositoryServices;
using RepositoryServices.Models;

namespace AdminPanel.Windows
{
    /// <summary>
    /// Interaction logic for UserDetailsWindow.xaml
    /// </summary>
    public partial class UserDetailsWindow
    {
        public UserDetailsWindow(int? userId)
        {
            Service = new ApplicationService();
            UserId = userId;
            Title = !UserId.HasValue ? "Create new user" : "Edit user";
            User = new UserModel();
            DataContext = User;
            InitializeComponent();
        }

        private ApplicationService Service { get; }

        private int? UserId { get; }

        public UserModel User { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (User.IsValid())
            {
                try
                {
                    var user = new User
                    {
                        FirstName = User.FirstName,
                        LastName = User.LastName,
                        Login = User.Login
                    };

                    if (UserId.HasValue)
                    {
                        Service.UserRepository.EditRecord(user, u => u.Id == UserId.Value);
                    }
                    else
                    {
                        Service.UserRepository.AddRecord(user);
                    }

                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ApplicationLoaded(object sender, RoutedEventArgs e)
        {
            if (UserId.HasValue)
            {
                var dbUser = Service.UserRepository.GetRecord(UserId.Value);
                if (dbUser == null)
                {
                    Close();
                    return;
                }

                User.FirstName = dbUser.FirstName;
                User.LastName = dbUser.LastName;
                User.LastLoginDate = dbUser.LastLoginDate;
            }
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Button_Click(sender, e);
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}