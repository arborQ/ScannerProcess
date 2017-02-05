using System.Threading.Tasks;

namespace Logger.Interfaces
{
    public interface ILogService
    {
        void InvalidLogin(string invalidLogin);

        void SuccesfulLogin(string validLogin);
    }
}