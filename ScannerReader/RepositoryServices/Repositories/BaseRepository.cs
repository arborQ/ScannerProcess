using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LiteDB;
using RepositoryServices.Interfaces;

namespace RepositoryServices.Repositories
{
    internal abstract class BaseRepository<T>
        where T : IBaseElement, new()
    {
        private const string DbFileName = "ApplicationDataBase.db";
        private readonly string _tableName;

        protected BaseRepository(string tableName)
        {
            _tableName = tableName;
            UniqueIndexes = new Expression<Func<T, string>>[0];
        }

        public virtual IEnumerable<T> GetRecords()
        {
            using (var db = new LiteDatabase(DbFileName))
            {
                var dbColletion = db.GetCollection<T>(_tableName);

                return dbColletion.FindAll();
            }
        }
        public virtual IEnumerable<T> GetRecords(Expression<Func<T, bool>> predicate)
        {
            using (var db = new LiteDatabase(DbFileName))
            {
                var dbColletion = db.GetCollection<T>(_tableName);

                return dbColletion.Find(predicate);
            }
        }

        public virtual T GetRecord(int id)
        {
            using (var db = new LiteDatabase(DbFileName))
            {
                var dbColletion = db.GetCollection<T>(_tableName);

                return dbColletion.FindById(id);
            }
        }

        public virtual void EditRecord(T sourceElement, Expression<Func<T, bool>> predicate)
        {
            using (var db = new LiteDatabase(DbFileName))
            {
                var dbColletion = db.GetCollection<T>(_tableName);
                var item = dbColletion.FindOne(predicate);
                UpdateElement(item, sourceElement);
                dbColletion.Update(item);

                foreach (var uniqueIndex in UniqueIndexes)
                {
                    dbColletion.EnsureIndex(uniqueIndex, true);
                }
            }
        }

        public virtual void EditRecord(T sourceElement)
        {
            EditRecord(sourceElement, arg => arg.Id == sourceElement.Id);
        }

        public virtual void AddRecord(T model)
        {
            using (var db = new LiteDatabase(DbFileName))
            {
                var dbColletion = db.GetCollection<T>(_tableName);
                dbColletion.Insert(model);
                foreach (var uniqueIndex in UniqueIndexes)
                {
                    dbColletion.EnsureIndex(uniqueIndex, true);
                }
            }
        }

        public void RemoveRecord(int recordId)
        {
            RemoveRecord(r => r.Id == recordId);
        }

        public void RemoveRecord(Expression<Func<T, bool>> predicate)
        {
            using (var db = new LiteDatabase(DbFileName))
            {
                var dbColletion = db.GetCollection<T>(_tableName);
                dbColletion.Delete(predicate);
            }
        }

        protected abstract void UpdateElement(T dbElement, T sourceElement);

        protected Expression<Func<T, string>>[] UniqueIndexes { get; set; }
    }
}