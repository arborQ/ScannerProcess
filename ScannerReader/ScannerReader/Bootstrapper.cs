using Common;
using Common.Interfaces;
using StructureMap;

namespace ScannerReader
{
    public class Bootstrapper
    {
        public static Container Container { get; private set; }

        public static void Initialize()
        {
            var container = new Container(_ =>
            {
                _.For<IKeyboardReader>().Use<KeyboardReader>();
                _.For<IUserSecurity>().Use<UserSecurity>();
            });

            Container = container;
        }
    }
}