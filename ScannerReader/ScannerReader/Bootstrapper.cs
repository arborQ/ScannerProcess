using Common;
using Common.Interfaces;
using StructureMap;
using StructureMap.Pipeline;

namespace ScannerReader
{
    public class Bootstrapper
    {
        public static Container Container { get; private set; }

        public static void Initialize()
        {
            var singletonLifecycle = new SingletonLifecycle();
            var container = new Container(_ =>
            {
                _.For<IKeyboardReader>().Use<KeyboardReader>();
#if DEBUG
                _.For<IUserSecurity>(singletonLifecycle).Use<DebugUserSecurity>();
#else
                _.For<IUserSecurity>(singletonLifecycle).Use<UserSecurity>();
#endif
            });

            Container = container;
        }
    }
}