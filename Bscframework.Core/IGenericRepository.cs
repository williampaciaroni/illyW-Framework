using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bscframework.Core
{
    public interface IGenericRepository<TEntity, T> : IGenericRepository
        where TEntity : class, IEntity<T>, new()
        where T : IComparable, IEquatable<T>
    {
        TEntity? GetSingle(T id);
        TEntity? GetSingle(Expression<Func<TEntity, bool>> condition);
        IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>>? condition = null);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }

    public interface IGenericRepository { }
}
