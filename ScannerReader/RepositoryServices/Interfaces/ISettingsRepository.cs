using RepositoryServices.Models;

namespace RepositoryServices.Interfaces
{
    public interface ISettingsRepository
    {
        Settings Get();

        void Update(Settings settings);
    }
}