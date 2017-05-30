using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using WorkflowService.Interfaces;
using WorkflowService.Providers;

namespace WorkflowService
{
    public class InstallServices
    {
        public static void Install(IWindsorContainer container)
        {
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
            container.Register(Component.For<IOrderCodeProvider>().ImplementedBy<OrderCodeProvider>().LifestyleTransient());
            container.Register(Component.For<IOrderCodeProvider>().ImplementedBy<TableCodeProvider>().LifestyleTransient());

            ControllerService.InstallServices.Install(container);
        }
    }
}
