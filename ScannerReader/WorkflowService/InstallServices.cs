using Castle.Windsor;

namespace WorkflowService
{
    public class InstallServices
    {
        public static void Install(IWindsorContainer container)
        {
            ControllerService.InstallServices.Install(container);
        }
    }
}
