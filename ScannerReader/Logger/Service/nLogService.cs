using System;
using Logger.Interfaces;
using NLog;
using NLog.Fluent;

namespace Logger.Service
{
    internal class nLogService : ILogService
    {
        private readonly NLog.Logger _logger;
        private readonly NLog.Logger _scanRegistry;
        private readonly NLog.Logger _loggerException;
        private readonly NLog.Logger _controllerActions;

        public nLogService()
        {
            _logger = LogManager.GetLogger("LogActivity");
            _scanRegistry = LogManager.GetLogger("ScanRegistry");
            _loggerException = LogManager.GetLogger("Exceptions");
            _controllerActions = LogManager.GetLogger("ControllerActions");
        }

        public void InvalidLogin(string invalidLogin)
        {
            _logger.Warn()
                .Message(invalidLogin)
                .Property("type", "INVALID")
                .Write();
        }

        public void SuccesfulLogin(string validLogin)
        {
            _logger.Info()
                .Message(validLogin)
                .Property("type", "VALID")
                .Write();
        }

        public void AdminFail()
        {
            _logger.Error()
                .Property("type", "ADMIN_PASSWORD_FAIL")
                .Write();
        }

        public void ApplicationStart()
        {
            _logger.Info()
                .Property("type", "APP_START")
                .Write();
        }

        public void ApplicationEnd()
        {
            _logger.Info()
                .Property("type", "APP_END")
                .Write();
        }

        public void Exception(Exception e)
        {
            _loggerException.Error(e);
        }

        public void FirstSuccesfulLogin(string validLogin)
        {
            _logger.Info()
               .Message(validLogin)
               .Property("type", "FIRST")
               .Write();
        }

        public void LogOut(string login, bool timeOut)
        {
            _logger.Info()
               .Message(login)
               .Property("type", timeOut ? "TIMEOUT": "LOGOUT")
               .Write();
        }

        public void ScanCode(string code)
        {
        }

        public void LogScaningDone(string login, string code, string engineCodeA, string engineCodeB, int? enginePositionA, int? enginePositionB, string programType)
        {
            _scanRegistry.Info()
               .Message(login)
               .Property("code", code)
               .Property("engineCodeA", engineCodeA)
               .Property("engineCodeB", engineCodeB)
               .Property("enginePositionA", enginePositionA.HasValue ? enginePositionA.Value.ToString(): string.Empty)
               .Property("enginePositionB", enginePositionB.HasValue ? enginePositionB.Value.ToString() : string.Empty)
               .Property("programType", programType)
               .Write();
        }

        public void ControllerAction(string message)
        {
            _scanRegistry.Info(message);
        }
    }
}