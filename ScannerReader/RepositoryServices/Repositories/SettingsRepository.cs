using System.IO;
using System.Linq;
using System.Reflection;
using RepositoryServices.Interfaces;
using RepositoryServices.Models;

namespace RepositoryServices.Repositories
{
    internal class SettingsRepository : BaseRepository<Settings>, ISettingsRepository
    {
        public SettingsRepository() : base("ApplicationSettings")
        {
        }

        public Settings Get()
        {
            var items = GetRecords().FirstOrDefault();
            if (items == null)
            {
                AddRecord(new Settings
                {
                    DefaultTimeout = 20,
                    ImagePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
                });
            }

            return GetRecords().First();
        }

        public void Update(Settings settings)
        {
            RemoveRecord(a => a.Id > 0);
            AddRecord(settings);
        }

        protected override void UpdateElement(Settings dbElement, Settings sourceElement)
        {
            dbElement.ImagePath = sourceElement.ImagePath;
            dbElement.DefaultTimeout = sourceElement.DefaultTimeout;
        }
    }
}