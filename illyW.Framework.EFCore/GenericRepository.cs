using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using illyW.Framework.Core.GenericEntityPattern;
using illyW.Framework.Core.RepositoryPattern;
using illyW.Framework.Core.ResultPattern;

namespace illyW.Framework.EFCore
{
    public abstract class GenericRepository<TEntity, T, TContext> : IGenericRepository<TEntity, T>
        where TEntity : class, IEntity<T>, new()
        where T : IComparable, IEquatable<T>
        where TContext : DbContext
    {
        private readonly TContext _context;
        protected readonly DbSet<TEntity> DbSet;

        protected TContext Context { get { return _context; } }

        public GenericRepository(TContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            DbSet = context.Set<TEntity>();
            _context = context;
        }

        public TEntity GetSingle(T id)
        {
            ArgumentNullException.ThrowIfNull(id);

            return DbSet.SingleOrDefault(x => x.Id.Equals(id));
        }
        
        public TEntity GetSingle(Expression<Func<TEntity, bool>> condition)
        {
            ArgumentNullException.ThrowIfNull(condition);

            return DbSet.SingleOrDefault(condition);
        }
        
        public Task<TEntity> GetSingleAsync(T id)
        {
            ArgumentNullException.ThrowIfNull(id);

            return DbSet.SingleOrDefaultAsync(x => x.Id.Equals(id));
        }
        
        public Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> condition)
        {
            ArgumentNullException.ThrowIfNull(condition);

            return DbSet.SingleOrDefaultAsync(condition);
        }

        public IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> condition = null)
        {
            return condition != null ? DbSet.Where(condition).AsEnumerable() : DbSet.AsEnumerable();
        }
        
        public IAsyncEnumerable<TEntity> FetchAsync(Expression<Func<TEntity, bool>> condition = null)
        {
            return condition != null ? DbSet.Where(condition).AsAsyncEnumerable() : DbSet.AsAsyncEnumerable();
        }

        public IResult Add(TEntity entity)
        {
            Result r = new();
            
            if (entity is null)
            {
                r.Fail();
                r.AddError($"Entity {typeof(TEntity).FullName} is null");
                return r;
            }

            try
            {
                DbSet.Add(entity);
                Context.SaveChanges();
                
                r.Succeed();
            }
            catch (Exception e)
            {
                r.Fail();
                r.AddError(e.Message);
            }

            return r;

        }
        
        public async Task<IResult> AddAsync(TEntity entity)
        {
            Result r = new();
            
            if (entity is null)
            {
                r.Fail();
                r.AddError($"Entity {typeof(TEntity).FullName} is null");
                return r;
            }

            try
            {
                await DbSet.AddAsync(entity);
                await Context.SaveChangesAsync();
                
                r.Succeed();
            }
            catch (Exception e)
            {
                r.Fail();
                r.AddError(e.Message);
            }

            return r;
        }

        public IResult Update(TEntity entity)
        {
            Result r = new();
            
            if (entity is null)
            {
                r.Fail();
                r.AddError($"Entity {typeof(TEntity).FullName} is null");
                return r;
            }

            try
            {
                DbSet.Update(entity);
                Context.SaveChanges();
                
                r.Succeed();
            }
            catch (Exception e)
            {
                r.Fail();
                r.AddError(e.Message);
            }

            return r;
        }
        
        public async Task<IResult> UpdateAsync(TEntity entity)
        {
            Result r = new();
            
            if (entity is null)
            {
                r.Fail();
                r.AddError($"Entity {typeof(TEntity).FullName} is null");
                return r;
            }

            try
            {
                DbSet.Update(entity);
                await Context.SaveChangesAsync();
                
                r.Succeed();
            }
            catch (Exception e)
            {
                r.Fail();
                r.AddError(e.Message);
            }

            return r;
        }

        public IResult Delete(TEntity entity)
        {
            Result r = new();
            
            if (entity is null)
            {
                r.Fail();
                r.AddError($"Entity {typeof(TEntity).FullName} is null");
                return r;
            }

            try
            {
                DbSet.Remove(entity);
                Context.SaveChanges();
                
                r.Succeed();
            }
            catch (Exception e)
            {
                r.Fail();
                r.AddError(e.Message);
            }

            return r;
        }
        
        public async Task<IResult> DeleteAsync(TEntity entity)
        {
            Result r = new();
            
            if (entity is null)
            {
                r.Fail();
                r.AddError($"Entity {typeof(TEntity).FullName} is null");
                return r;
            }

            try
            {
                DbSet.Remove(entity);
                await Context.SaveChangesAsync();
                
                r.Succeed();
            }
            catch (Exception e)
            {
                r.Fail();
                r.AddError(e.Message);
            }

            return r;
        }
    }
}
