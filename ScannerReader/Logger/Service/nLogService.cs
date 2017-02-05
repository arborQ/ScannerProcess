using Logger.Interfaces;
using NLog;

namespace Logger.Service
{
    internal class nLogService : ILogService
    {
        private readonly NLog.Logger _logger;

        public nLogService()
        {
            _logger = LogManager.GetLogger("MyClassName");
        }

        public void InvalidLogin(string invalidLogin)
        {
            _logger.Log(LogLevel.Warn, $"Invalid login: {invalidLogin}");
        }

        public void SuccesfulLogin(string validLogin)
        {
            _logger.Log(LogLevel.Info, $"Valid login: {validLogin}");
        }
    }
}