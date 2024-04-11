using illyW.Framework.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace illyW.Framework.EFCore.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static IServiceCollection AddRepository<TRepositoryInterface, TRepositoryImplementation>(this IServiceCollection serviceCollection)
            where TRepositoryInterface : class, IGenericRepository
            where TRepositoryImplementation : class, TRepositoryInterface
        {
            serviceCollection.AddScoped<TRepositoryInterface, TRepositoryImplementation>();
            serviceCollection.AddScoped<IGenericRepository, TRepositoryImplementation>();
            return serviceCollection;
        }

        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection serviceCollection)
            where TContext : DbContext
        {
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            return serviceCollection;
        }

    }
}
