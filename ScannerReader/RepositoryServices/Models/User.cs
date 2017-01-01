using System;
using RepositoryServices.Interfaces;

namespace RepositoryServices.Models
{
    public class User : IUser
    {
        public int Id { get; set; }

        public virtual string Login { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual DateTime? LastLoginDate { get; set; }

        public string PasswordHash { get; set; }
    }
}