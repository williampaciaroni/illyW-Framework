using System;
using System.Linq;
using EventStore.Client;
using illyW.Framework.EventStore.Persistence;
using illyW.Framework.EventStore.Rehydrators;
using illyW.Framework.EventStore.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace illyW.Framework.EventStore.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static IServiceCollection EnableEventStore<TContext>(this IServiceCollection services, IConfiguration configuration, string eventStoreConnectionString = null) where TContext : EventStoreDbContext
        {
            eventStoreConnectionString ??= configuration.GetConnectionString("EventStore");

            ArgumentException.ThrowIfNullOrWhiteSpace(eventStoreConnectionString);

            services.AddSingleton(new EventStoreClient(EventStoreClientSettings.Create(eventStoreConnectionString)));

            services.AddScoped<ICheckpointRepository, CheckpointRepository<TContext>>();
            services.AddScoped<IStreamCheckpointRepository, StreamCheckpointRepository<TContext>>();
            services.AddScoped<IAllCheckpointRepository, AllCheckpointRepository<TContext>>();

            return services;
        }

        public static IServiceCollection AddRehydrator<TRehydrator>(this IServiceCollection serviceCollection)
            where TRehydrator : BaseRehydrator
        {
            if (serviceCollection.FirstOrDefault(x => x.ServiceType == typeof(EventStoreClient)) == null)
            {
                throw new Exception("Enable Event Store before add rehydrator");
            }

            serviceCollection.AddHostedService<TRehydrator>();

            return serviceCollection;
        }

    }
}
