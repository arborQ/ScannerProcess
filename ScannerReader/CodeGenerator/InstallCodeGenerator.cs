using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace CodeGenerator
{
    public class InstallCodeGenerator
    {
        public static void Install(IWindsorContainer container)
        {
            container.Register(Component.For<ICodeGenerator>().ImplementedBy<QrCodeGenerator>().LifestyleTransient());
        }
    }
}