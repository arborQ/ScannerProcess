using RepositoryServices.Interfaces;
using RepositoryServices.Repositories;

namespace RepositoryServices
{
    public class ApplicationService
    {
        private UserRepository _userRepository;
        public IUserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository());
    }
}