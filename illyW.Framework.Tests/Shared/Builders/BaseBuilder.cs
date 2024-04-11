using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using illyW.Framework.Tests.Shared.Attributes;
using Microsoft.EntityFrameworkCore;

namespace illyW.Framework.Tests.Shared.Builders;

public class BaseBuilder<T> : ISpecimenBuilder
{
    public virtual object Create(object request, ISpecimenContext context)
    {
        var customAttributeProvider = request as ICustomAttributeProvider;
        
        if (customAttributeProvider == null)
        {
            return new NoSpecimen();
        }
        
        var dbAttribute =
            customAttributeProvider.GetCustomAttributes(typeof(DbAttribute), true)
                .OfType<DbAttribute>()
                .FirstOrDefault();
        
        var dbContext = context.Create<DbContext>();

        if (dbAttribute != null)
        {
            dbContext.Add(request);
        }

        return request;
    }
}