using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Remake_CB_4_Project_1.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);


        void SignOut();
        void ExitApp();
        void MenuBack();
        string ReadUserInput();
    }
}
