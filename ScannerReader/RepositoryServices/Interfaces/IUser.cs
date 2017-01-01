using System;

namespace RepositoryServices.Interfaces
{
    public interface IUser : IBaseElement
    {
        string Login { get; }

        string FirstName { get; }

        string LastName { get; }

        DateTime? LastLoginDate { get; }

        string PasswordHash { get; }
    }
}