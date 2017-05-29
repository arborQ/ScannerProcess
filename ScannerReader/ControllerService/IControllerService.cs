using System.Threading.Tasks;

namespace ControllerService
{
    public interface IControllerService
    {
        Task<bool> SelectJobAsync(int jobNumber);
    }

    public interface IControllerServiceFactory
    {
        IControllerService Create(ControllerEvents events, string addressIp);
    }
}
