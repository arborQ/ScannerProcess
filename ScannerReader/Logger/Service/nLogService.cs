using System;
using Logger.Interfaces;
using NLog;
using NLog.Fluent;

namespace Logger.Service
{
    internal class nLogService : ILogService
    {
        private readonly NLog.Logger _logger;
        private readonly NLog.Logger _loggerException;

        public nLogService()
        {
            _logger = LogManager.GetLogger("LogActivity");
            _loggerException = LogManager.GetLogger("Exceptions");
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
    }
}