using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using illyW.Framework.Core.GenericEntityPattern;
using illyW.Framework.Core.ResultPattern;

namespace illyW.Framework.Core.RepositoryPattern
{
    public interface IGenericRepository<TEntity, in T> : IGenericRepository
        where TEntity : class, IEntity<T>, new() 
        where T : IComparable, IEquatable<T>
    {
        TEntity GetSingle(T id);
        TEntity GetSingle(Expression<Func<TEntity, bool>> condition);
        Task<TEntity> GetSingleAsync(T id);
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> condition);
        IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> condition = null);
        IAsyncEnumerable<TEntity> FetchAsync(Expression<Func<TEntity, bool>> condition = null);
        IResult Add(TEntity entity);
        Task<IResult> AddAsync(TEntity entity);
        IResult Update(TEntity entity);
        Task<IResult> UpdateAsync(TEntity entity);
        IResult Delete(TEntity entity);
        Task<IResult> DeleteAsync(TEntity entity);
    }

    public interface IGenericRepository { }
}
