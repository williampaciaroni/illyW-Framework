using EventStore.Client;
using illyW.Framework.Core.RepositoryPattern;
using illyW.Framework.EFCore;
using illyW.Framework.EventStore.Rehydrators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace illyW.Framework.EventStore.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static IServiceCollection AddEventStore(this IServiceCollection serviceCollection, string connectionString)
        {
            var settings = EventStoreClientSettings.Create(connectionString);

            var client = new EventStoreClient(settings);

            serviceCollection.AddSingleton(client);
            
            return serviceCollection;
        }

        public static IServiceCollection AddRehydrator<TRehydrator>(this IServiceCollection serviceCollection)
            where TRehydrator : BaseRehydrator
        {
            serviceCollection.AddHostedService<TRehydrator>();
            return serviceCollection;
        }

    }
}
