using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RepositoryServices.Interfaces
{
    public interface IRepository<T> where T : IBaseElement, new()
    {
        IEnumerable<T> GetRecords();

        IEnumerable<T> GetRecords(Expression<Func<T, bool>> predicate);

        T GetRecord(int id);

        void EditRecord(T model);

        void EditRecord(T sourceElement, Expression<Func<T, bool>> predicate);

        void AddRecord(T model);

        void RemoveRecord(int recordId);

        void RemoveRecord(Expression<Func<T, bool>> predicate);
    }
}