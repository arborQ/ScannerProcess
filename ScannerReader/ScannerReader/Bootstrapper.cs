using System.Windows;
using System.Windows.Controls;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common;
using Common.Interfaces;
using Logger;
using RepositoryServices;
using ScannerReader.Interfaces;
using ScannerReader.Services;
using WorkflowService;
using WorkflowService.Interfaces;
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

            Container.Register(Classes.FromThisAssembly().BasedOn<Window>().LifestyleTransient());
            Container.Register(Classes.FromThisAssembly().BasedOn<UserControl>().LifestyleTransient());
            Container.Register(Component.For<IControlFactory>().AsFactory());
            Container.Register(Component.For<IWindowFactory>().AsFactory());
            //Container.Register(Classes.FromAssembly(Assembly.Load(nameof(AdminPanel))).BasedOn<System.Windows.Window>());
            Container.Register(Component.For<Workflow>().LifestyleTransient());

            Container.Register(Component.For<IReadValueService>().ImplementedBy<ReadValueService>());

            Container.Register(Component.For<IWorkflowStateFactory>().AsFactory());

            Container.Register(Component.For<IWorkflowState>().LifestyleTransient()
                .ImplementedBy<PendingWorklowState>()
                .Named("PendingState"));

            Container.Register(Component.For<IWorkflowState>().LifestyleTransient()
                .ImplementedBy<SingleEnginePompState>()
                .Named("SingleEngineState"));

            Container.Register(Component.For<IWorkflowState>().LifestyleTransient()
                .ImplementedBy<MultipleEnginePompState>()
                .Named("MultipleEngineState"));

            Container.Register(Component.For<IWorkflowState>().LifestyleTransient()
                .ImplementedBy<DisplayMachineDataState>()
                .Named("DisplayMachineDataState"));

            Container.Register(Component.For<IKeyboardReader>().ImplementedBy<KeyboardReader>());
#if DEBUG
            Container.Register(Component.For<IUserSecurity>().ImplementedBy<UserSecurity>().LifestyleSingleton());
#else
            Container.Register(Component.For<IUserSecurity>().ImplementedBy<UserSecurity>().LifestyleSingleton());
#endif

            InstallServices.Install(Container);
            InstallRepositories.Install(Container);
        }

        public static T Resolve<T>() where T : class
        {
            return Container.Resolve<T>();
        }
    }
}