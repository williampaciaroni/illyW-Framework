using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq.Expressions;

namespace illyW.Framework.Core
{
    public interface IGenericRepository<TEntity, in T> : IGenericRepository
        where TEntity : class, IEntity<T>, INullable, new() 
        where T : IComparable, IEquatable<T>
    {
        TEntity GetSingle(T id);
        TEntity GetSingle(Expression<Func<TEntity, bool>> condition);
        IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> condition = null);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }

    public interface IGenericRepository { }
}
