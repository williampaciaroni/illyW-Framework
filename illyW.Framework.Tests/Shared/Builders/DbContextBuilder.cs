using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace illyW.Framework.Tests.Shared.Builders
{
    public class DbContextBuilder<TContext> : ISpecimenBuilder
        where TContext : DbContext
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is not Type type || type != typeof(TContext)) return new NoSpecimen();

            var options = new DbContextOptionsBuilder<TContext>()
                .UseInMemoryDatabase(context.Create<string>())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return Activator.CreateInstance(typeof(TContext), new object[] { options })!;
        }
    }
}