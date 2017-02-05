using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Logger.Interfaces;
using Logger.Service;

namespace Logger
{
    public class InstallServices
    {
        public static void Install(IWindsorContainer container)
        {
            //container.Register(Component.For<ILogService>().ImplementedBy<LogService>().LifestyleSingleton());
            container.Register(Component.For<ILogService>().ImplementedBy<nLogService>().LifestyleSingleton());
            
        }
    }
}