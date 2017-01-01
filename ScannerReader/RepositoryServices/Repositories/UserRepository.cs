using System;
using System.Linq.Expressions;
using RepositoryServices.Interfaces;
using RepositoryServices.Models;

namespace RepositoryServices.Repositories
{
    internal class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository() : base("Users")
        {
            UniqueIndexes = new Expression<Func<User, string>>[]
            {
                u => u.Login
            };
        }

        protected override void UpdateElement(User dbElement, User sourceElement)
        {
            dbElement.FirstName = sourceElement.FirstName;
            dbElement.LastName = sourceElement.LastName;
            dbElement.Login = sourceElement.Login;
        }
    }
}