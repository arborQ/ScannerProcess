using System;
using System.Threading.Tasks;

namespace Logger.Interfaces
{
    public interface ILogService
    {
        void InvalidLogin(string invalidLogin);

        void SuccesfulLogin(string validLogin);

        void AdminFail();

        void ApplicationStart();

        void ApplicationEnd();

        void Exception(Exception e);
    }
}