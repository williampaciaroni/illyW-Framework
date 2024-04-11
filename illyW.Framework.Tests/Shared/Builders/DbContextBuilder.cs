using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace illyW.Framework.Tests.Shared.Builders
{
    public class DbContextBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(DbContext))
            {
                var options = new DbContextOptionsBuilder<DbContext>()
                    .UseInMemoryDatabase(context.Create<string>())
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .Options;

                return new DbContext(options);
            }

            return new NoSpecimen();
        }
    }
}
