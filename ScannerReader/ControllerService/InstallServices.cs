using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace ControllerService
{
    public class InstallServices
    {
        public static void Install(IWindsorContainer container)
        {
            container.Register(Component.For<IControllerService>().ImplementedBy<ControllerService>().LifestyleTransient());
            container.Register(Component.For<IControllerServiceFactory>().AsFactory());
        }
    }
}
