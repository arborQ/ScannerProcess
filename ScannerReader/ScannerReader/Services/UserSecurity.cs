using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Logger.Interfaces;
using RepositoryServices;
using CrossCutting;

namespace ScannerReader.Services
{
    public class UserSecurity : IUserSecurity
    {
        private readonly ApplicationService _applicationService;
        private readonly ILogService _logger;
        private string _currentLogin;
        public UserSecurity(ApplicationService applicationService, ILogService logger)
        {
            _applicationService = applicationService;
            _logger = logger;
        }
        public bool ValidateUser(string login, string password)
        {
            var user = _applicationService.UserRepository.GetRecords(u => u.Login == login).SingleOrDefault();

            if (user == null)
            {
                _logger.InvalidLogin(login);

                return false;
            }

            var passwordHash = HashPassword(password);

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                _logger.FirstSuccesfulLogin(login);
                _applicationService.UserRepository.EditRecord(u => u.Login == login, u =>
                {
                    u.PasswordHash = passwordHash;
                    u.LastLoginDate = DateTime.UtcNow;
                });

                return true; //it's new user, first loggin
            }

            if (user.PasswordHash == passwordHash)
            {
                _logger.SuccesfulLogin(login);
                return true;
            }

            _logger.InvalidLogin(login);

            return false;
        }

        public void SetCurrentUser(string login)
        {
            _currentLogin = login;
        }

        public string CurrentUserLogin()
        {
            return _currentLogin;
        }

        private string HashPassword(string password)
        {
            var data = Encoding.ASCII.GetBytes(password);
            var md5 = new MD5CryptoServiceProvider();
            var md5Data = md5.ComputeHash(data);
            return Encoding.ASCII.GetString(md5Data);
        }

        public bool ValidateBarCodeUser(string login)
        {
            return _applicationService.UserRepository.GetRecords(u => u.Login == login).SingleOrDefault() != null;
        }
    }
}