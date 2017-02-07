using System;
using System.Linq.Expressions;
using RepositoryServices.Interfaces;
using RepositoryServices.Models;

namespace RepositoryServices.Repositories
{
    internal class MachineRepository : BaseRepository<Machine>, IMachineRepository
    {
        public MachineRepository() : base("Machines")
        {
            UniqueIndexes = new Expression<Func<Machine, string>>[] { r => r.Code };
        }

        protected override void UpdateElement(Machine dbElement, Machine sourceElement)
        {
        }
    }
}