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
            //container.Register(Classes.FromAssemblyContaining<IOrderCodeProvider>().BasedOn<IOrderCodeProvider>().LifestyleTransient());
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
            container.Register(Component.For<IOrderCodeProvider>().ImplementedBy<OrderCodeProvider>().LifestyleTransient());
            container.Register(Component.For<IOrderCodeProvider>().ImplementedBy<TableCodeProvider>().LifestyleTransient());
            //container.Register(
            //Classes.FromAssemblyContaining<IOrderCodeProvider>()
            //    .BasedOn<IOrderCodeProvider>().WithService.FromInterface()
            //);

            ControllerService.InstallServices.Install(container);
        }
    }
}
