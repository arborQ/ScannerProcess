using System.Reflection;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common;
using Common.Interfaces;
using RepositoryServices;
using WorkflowService;
using WorkflowService.States;

namespace ScannerReader
{
    public class Bootstrapper
    {
        public static IWindsorContainer Container { get; private set; }

        public static void Initialize()
        {
            Container = new WindsorContainer();
            Container.AddFacility<TypedFactoryFacility>();

            Container.Register(Classes.FromThisAssembly().BasedOn<System.Windows.Window>());
            Container.Register(Classes.FromAssembly(Assembly.Load(nameof(AdminPanel))).BasedOn<System.Windows.Window>());
            Container.Register(Component.For<Workflow>());
            Container.Register(Component.For<ApplicationService>());

            Container.Register(Component.For<IWorkflowStateFactory>().AsFactory());

            Container.Register(Component.For<IWorkflowState>()
                .ImplementedBy<PendingWorklowState>()
                .Named("PendingState"));

            Container.Register(Component.For<IWorkflowState>()
                .ImplementedBy<SingleEnginePompState>()
                .Named("SingleEngineState"));

            Container.Register(Component.For<IWorkflowState>()
                .ImplementedBy<MultipleEnginePompState>()
                .Named("MultipleEngineState"));

            Container.Register(Component.For<IWorkflowState>()
                .ImplementedBy<DisplayMachineDataState>()
                .Named("DisplayMachineDataState"));

            Container.Register(Component.For<IKeyboardReader>().ImplementedBy<KeyboardReader>());
#if DEBUG
            Container.Register(Component.For<IUserSecurity>().ImplementedBy<DebugUserSecurity>().LifestyleSingleton());
#else
            Container.Register(Component.For<IUserSecurity>().ImplementedBy<UserSecurity>().LifestyleSingleton());
#endif
        }

        public static T Resolve<T>() where T : class
        {
            return Container.Resolve<T>();
        }
    }
}