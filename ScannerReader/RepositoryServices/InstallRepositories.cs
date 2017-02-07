using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using RepositoryServices.Interfaces;
using RepositoryServices.Repositories;

namespace RepositoryServices
{
    public class InstallRepositories
    {
        public static void Install(IWindsorContainer container)
        {
            //container.Register(Component.For<ILogService>().ImplementedBy<LogService>().LifestyleSingleton());
            container.Register(Component.For<IUserRepository>().ImplementedBy<UserRepository>().LifestyleSingleton());
            container.Register(Component.For<IMachineRepository>().ImplementedBy<MachineRepository>().LifestyleSingleton());
            container.Register(Component.For<ISettingsRepository>().ImplementedBy<SettingsRepository>().LifestyleTransient());
            container.Register(Component.For<ApplicationService>().LifestyleSingleton());

            


        }
    }
}
