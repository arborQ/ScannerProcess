﻿using System;

namespace Logger.Interfaces
{
    public interface ILogService
    {
        void InvalidLogin(string invalidLogin);

        void SuccesfulLogin(string validLogin);

        void FirstSuccesfulLogin(string validLogin);

        void LogOut(string login, bool timeOut);

        void AdminFail();

        void ApplicationStart();

        void ApplicationEnd();

        void Exception(Exception e);

        void ScanCode(string code);

    }
}