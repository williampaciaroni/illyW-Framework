using System.Collections.ObjectModel;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using illyW.Framework.Tests.Shared.Attributes;
using Microsoft.EntityFrameworkCore;
using CollectionAttribute = illyW.Framework.Tests.Shared.Attributes.CollectionAttribute;

namespace illyW.Framework.Tests.Shared.Builders;

public abstract class BaseBuilder<T, TContext> : ISpecimenBuilder
where TContext : DbContext 
where T : class
{
    public object Create(object request, ISpecimenContext context)
    {
        var customAttributeProvider = request as  ICustomAttributeProvider;

        if (customAttributeProvider == null)
        {
            return new NoSpecimen();
        }
        
        if (!typeof(T).Equals(request))
        {
            return new NoSpecimen();
        }
        
        // TODO: fix custom attributes
        var dbAttribute =
            customAttributeProvider.GetCustomAttributes(typeof(DbAttribute), true)
                .OfType<DbAttribute>()
                .FirstOrDefault();

        var collectionAttribute =
            customAttributeProvider.GetCustomAttributes(typeof(CollectionAttribute), true)
                .OfType<CollectionAttribute>()
                .FirstOrDefault();

        if (collectionAttribute != null)
        {
            var collection = new Collection<T>();
            for (var i = 0; i < collectionAttribute.Count; i++)
            {
                collection.Add(CreateItem(context));
            }

            if (dbAttribute is not null)
            {
                var dbContext = context.Create<TContext>();
                
                dbContext.Set<T>().AddRange(collection);
                dbContext.SaveChanges();
            }

            return collection;
        }
        
        T item = CreateItem(context);

        if (dbAttribute is not null)
        {
            var dbContext = context.Create<TContext>();
                
            dbContext.Set<T>().Add(item);
            dbContext.SaveChanges();
        }

        return item;
    }

    protected abstract T CreateItem(ISpecimenContext context);
}