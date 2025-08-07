using System;
using System.Collections.Generic;
using System.Linq;
using illyW.Framework.Core.GenericEntityPattern;
using Microsoft.EntityFrameworkCore;

namespace illyW.Framework.EFCore.Extensions
{
    internal static class DbSetExtensions
    {
        public static IQueryable<TEntity> IncludeProperties<TEntity, T>(this DbSet<TEntity> dbSet,
            IList<string> includedProperties)
            where TEntity : class, IEntity<T>, new()
            where T : IComparable, IEquatable<T>
        {
            var set = dbSet.AsQueryable();

            if (includedProperties is null || !includedProperties.Any())
            {
                return set;
            }

            return includedProperties.Aggregate(set, (current, property) => current.Include(property));
        }
    }
}