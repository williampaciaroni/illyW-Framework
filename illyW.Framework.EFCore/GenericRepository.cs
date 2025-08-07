using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using illyW.Framework.Core.GenericEntityPattern;
using illyW.Framework.Core.RepositoryPattern;
using illyW.Framework.Core.ResultPattern;
using illyW.Framework.EFCore.Extensions;

namespace illyW.Framework.EFCore
{
    public abstract class GenericRepository<TEntity, T, TContext> : IGenericRepository<TEntity, T>
        where TEntity : class, IEntity<T>, new()
        where T : IComparable, IEquatable<T>
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbSet;

        private TContext Context
        {
            get { return _context; }
        }

        protected GenericRepository(TContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            _dbSet = context.Set<TEntity>();
            _context = context;
        }

        /// <summary>
        /// Retrieves a single entity from the database by its identifier, 
        /// optionally including specified navigation properties.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity to retrieve.</typeparam>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <param name="includedProperties">
        /// (Optional) A list of navigation property names to include in the query.  
        /// Nested properties can be specified using dot notation (e.g., "Parent.Child").
        /// </param>
        /// <returns>
        /// The entity matching the given identifier, or <c>null</c> if not found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="id"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This method uses <c>SingleOrDefault</c>, so it will return <c>null</c> 
        /// if no entity is found, or throw an exception if multiple matches are found.
        ///
        /// <para>Example usage:</para>
        /// <code>
        /// var singleEntity = repository.GetSingle(entity.Id, new List&lt;string&gt;
        /// {
        ///     $"{nameof(TestEntity.TestEntity2)}.{nameof(TestEntity.TestEntity2.TestEntity3)}"
        /// });
        /// </code>
        /// <para>
        /// In this example, both <c>TestEntity2</c> and the nested <c>TestEntity2.TestEntity3</c>
        /// navigation properties are eagerly loaded, allowing access to related entities directly.
        /// </para>
        /// </remarks>
        public TEntity GetSingle(T id, IList<string> includedProperties = null)
        {
            ArgumentNullException.ThrowIfNull(id);

            return _dbSet.IncludeProperties<TEntity, T>(includedProperties).SingleOrDefault(x => x.Id.Equals(id));
        }
        
        /// <summary>
        /// Retrieves a single entity from the database that matches the specified condition,
        /// optionally including specified navigation properties.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity to retrieve.</typeparam>
        /// <param name="condition">A LINQ expression that defines the condition the entity must satisfy (e.g., <c>x => x.Id == 5</c>).</param>
        /// <param name="includedProperties">
        /// (Optional) A list of navigation property names to include in the query.  
        /// Nested properties can be specified using dot notation (e.g., "Parent.Child").
        /// </param>
        /// <returns>
        /// The entity matching the given condition, or <c>null</c> if not found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="condition"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This method uses <c>SingleOrDefault</c>, so it will return <c>null</c> 
        /// if no entity is found, or throw an exception if multiple matches are found.
        ///
        /// <para>Example usage:</para>
        /// <code>
        /// var singleEntity = repository.GetSingle(x => x.Id == 1, new List&lt;string&gt;
        /// {
        ///     $"{nameof(TestEntity.TestEntity2)}.{nameof(TestEntity.TestEntity2.TestEntity3)}"
        /// });
        /// </code>
        /// <para>
        /// In this example, both <c>TestEntity2</c> and the nested <c>TestEntity2.TestEntity3</c>
        /// navigation properties are eagerly loaded, allowing access to related entities directly.
        /// </para>
        /// </remarks>
        public TEntity GetSingle(Expression<Func<TEntity, bool>> condition, IList<string> includedProperties = null)
        {
            ArgumentNullException.ThrowIfNull(condition);

            return _dbSet.IncludeProperties<TEntity, T>(includedProperties).SingleOrDefault(condition);
        }

        /// <summary>
        /// Retrieves a single entity from the database by its identifier, 
        /// optionally including specified navigation properties.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity to retrieve.</typeparam>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <param name="includedProperties">
        /// (Optional) A list of navigation property names to include in the query.  
        /// Nested properties can be specified using dot notation (e.g., "Parent.Child").
        /// </param>
        /// <returns>
        /// The entity matching the given identifier, or <c>null</c> if not found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="id"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This method uses <c>SingleOrDefault</c>, so it will return <c>null</c> 
        /// if no entity is found, or throw an exception if multiple matches are found.
        ///
        /// <para>Example usage:</para>
        /// <code>
        /// var singleEntity = await repository.GetSingleAsync(entity.Id, new List&lt;string&gt;
        /// {
        ///     $"{nameof(TestEntity.TestEntity2)}.{nameof(TestEntity.TestEntity2.TestEntity3)}"
        /// });
        /// </code>
        /// <para>
        /// In this example, both <c>TestEntity2</c> and the nested <c>TestEntity2.TestEntity3</c>
        /// navigation properties are eagerly loaded, allowing access to related entities directly.
        /// </para>
        /// </remarks>
        public Task<TEntity> GetSingleAsync(T id, IList<string> includedProperties = null)
        {
            ArgumentNullException.ThrowIfNull(id);

            return _dbSet.IncludeProperties<TEntity, T>(includedProperties).SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        /// <summary>
        /// Retrieves a single entity from the database that matches the specified condition,
        /// optionally including specified navigation properties.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity to retrieve.</typeparam>
        /// <param name="condition">A LINQ expression that defines the condition the entity must satisfy (e.g., <c>x => x.Id == 5</c>).</param>
        /// <param name="includedProperties">
        /// (Optional) A list of navigation property names to include in the query.  
        /// Nested properties can be specified using dot notation (e.g., "Parent.Child").
        /// </param>
        /// <returns>
        /// The entity matching the given condition, or <c>null</c> if not found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="condition"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This method uses <c>SingleOrDefault</c>, so it will return <c>null</c> 
        /// if no entity is found, or throw an exception if multiple matches are found.
        ///
        /// <para>Example usage:</para>
        /// <code>
        /// var singleEntity = await repository.GetSingleAsync(x => x.Id == 1, new List&lt;string&gt;
        /// {
        ///     $"{nameof(TestEntity.TestEntity2)}.{nameof(TestEntity.TestEntity2.TestEntity3)}"
        /// });
        /// </code>
        /// <para>
        /// In this example, both <c>TestEntity2</c> and the nested <c>TestEntity2.TestEntity3</c>
        /// navigation properties are eagerly loaded, allowing access to related entities directly.
        /// </para>
        /// </remarks>
        public Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> condition,
            IList<string> includedProperties = null)
        {
            ArgumentNullException.ThrowIfNull(condition);

            return _dbSet.IncludeProperties<TEntity, T>(includedProperties).SingleOrDefaultAsync(condition);
        }

        /// <summary>
        /// Retrieves one or more entities from the database that matches the specified condition,
        /// optionally including specified navigation properties.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity to retrieve.</typeparam>
        /// <param name="condition">A LINQ expression that defines the condition the entity must satisfy (e.g., <c>x => x.Value > 5</c>).</param>
        /// <param name="includedProperties">
        /// (Optional) A list of navigation property names to include in the query.  
        /// Nested properties can be specified using dot notation (e.g., "Parent.Child").
        /// </param>
        /// <returns>
        /// The entities matching the given condition.
        /// </returns>
        /// <remarks>
        /// <para>Example usage:</para>
        /// <code>
        /// var singleEntity = repository.Fetch(x => x.Value > 5, new List&lt;string&gt;
        /// {
        ///     $"{nameof(TestEntity.TestEntity2)}.{nameof(TestEntity.TestEntity2.TestEntity3)}"
        /// });
        /// </code>
        /// <para>
        /// In this example, both <c>TestEntity2</c> and the nested <c>TestEntity2.TestEntity3</c>
        /// navigation properties are eagerly loaded, allowing access to related entities directly.
        /// </para>
        /// </remarks>
        public IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>> condition = null,
            IList<string> includedProperties = null)
        {
            var q = _dbSet.IncludeProperties<TEntity, T>(includedProperties);

            return condition != null ? q.Where(condition).AsEnumerable() : q.AsEnumerable();
        }

        /// <summary>
        /// Retrieves one or more entities from the database that matches the specified condition,
        /// optionally including specified navigation properties.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity to retrieve.</typeparam>
        /// <param name="condition">A LINQ expression that defines the condition the entity must satisfy (e.g., <c>x => x.Value > 5</c>).</param>
        /// <param name="includedProperties">
        /// (Optional) A list of navigation property names to include in the query.  
        /// Nested properties can be specified using dot notation (e.g., "Parent.Child").
        /// </param>
        /// <returns>
        /// The entities matching the given condition.
        /// </returns>
        /// <remarks>
        /// <para>Example usage:</para>
        /// <code>
        /// var singleEntity = await repository.FetchAsync(x => x.Value > 5, new List&lt;string&gt;
        /// {
        ///     $"{nameof(TestEntity.TestEntity2)}.{nameof(TestEntity.TestEntity2.TestEntity3)}"
        /// });
        /// </code>
        /// <para>
        /// In this example, both <c>TestEntity2</c> and the nested <c>TestEntity2.TestEntity3</c>
        /// navigation properties are eagerly loaded, allowing access to related entities directly.
        /// </para>
        /// </remarks>
        public IAsyncEnumerable<TEntity> FetchAsync(Expression<Func<TEntity, bool>> condition = null,
            IList<string> includedProperties = null)
        {
            var q = _dbSet.IncludeProperties<TEntity, T>(includedProperties);

            return condition != null ? q.Where(condition).AsAsyncEnumerable() : q.AsAsyncEnumerable();
        }

        public IResult<TEntity> Add(TEntity entity)
        {
            Result<TEntity> r = new();

            if (entity is null)
            {
                r.Fail();
                r.AddError($"Entity {typeof(TEntity).FullName} is null");
                return r;
            }

            try
            {
                _dbSet.Add(entity);
                Context.SaveChanges();

                r.Succeed();
                r.Data = entity;
            }
            catch (Exception e)
            {
                r.Fail();
                r.AddError(e.Message);
            }

            return r;
        }

        public async Task<IResult<TEntity>> AddAsync(TEntity entity)
        {
            Result<TEntity> r = new();

            if (entity is null)
            {
                r.Fail();
                r.AddError($"Entity {typeof(TEntity).FullName} is null");
                return r;
            }

            try
            {
                await _dbSet.AddAsync(entity);
                await Context.SaveChangesAsync();

                r.Succeed();
                r.Data = entity;
            }
            catch (Exception e)
            {
                r.Fail();
                r.AddError(e.Message);
            }

            return r;
        }

        public IResult<TEntity> Update(TEntity entity)
        {
            Result<TEntity> r = new();

            if (entity is null)
            {
                r.Fail();
                r.AddError($"Entity {typeof(TEntity).FullName} is null");
                return r;
            }

            try
            {
                _dbSet.Update(entity);
                Context.SaveChanges();

                r.Succeed();
                r.Data = entity;
            }
            catch (Exception e)
            {
                r.Fail();
                r.AddError(e.Message);
            }

            return r;
        }

        public async Task<IResult<TEntity>> UpdateAsync(TEntity entity)
        {
            Result<TEntity> r = new();

            if (entity is null)
            {
                r.Fail();
                r.AddError($"Entity {typeof(TEntity).FullName} is null");
                return r;
            }

            try
            {
                _dbSet.Update(entity);
                await Context.SaveChangesAsync();

                r.Succeed();
                r.Data = entity;
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
                _dbSet.Remove(entity);
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
                _dbSet.Remove(entity);
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