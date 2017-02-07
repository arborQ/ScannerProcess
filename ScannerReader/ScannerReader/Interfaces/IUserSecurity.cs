﻿namespace Common.Interfaces
{
    public interface IUserSecurity
    {
        void SetCurrentUser(string login);

        bool ValidateUser(string login, string password);

        string CurrentUserLogin();
    }
}