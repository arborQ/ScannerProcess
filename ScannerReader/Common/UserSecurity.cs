using System;
using Common.Interfaces;

namespace Common
{
    public class UserSecurity : IUserSecurity
    {
        private string currentLogin;

        public void SetCurrentUser(string login)
        {
            currentLogin = login;
        }

        public string CurrentUserLogin()
        {
            if (string.IsNullOrEmpty(currentLogin))
            {
                throw new Exception("User is not authenticated");
            }

            return currentLogin;
        }
    }
}