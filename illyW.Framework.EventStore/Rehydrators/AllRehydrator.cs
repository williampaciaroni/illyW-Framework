using System;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using illyW.Framework.EventStore.Entities;
using illyW.Framework.EventStore.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace illyW.Framework.EventStore.Rehydrators
{
    public abstract class AllRehydrator(IServiceProvider serviceProvider, EventStoreClient client) : BaseRehydrator
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Subscribe(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(500, stoppingToken);
            }

            await client.DisposeAsync();
        }

        internal override async Task Subscribe(CancellationToken stoppingToken)
        {
            var checkpoint = ReadStreamCheckpoint() switch
            {
                null => FromAll.Start,
                var position => FromAll.After((Position)position)
            };

            try
            {
                var subscription = client.SubscribeToAll(checkpoint, cancellationToken: stoppingToken);

                await foreach (var message in subscription.Messages)
                {
                    switch (message)
                    {
                        case StreamMessage.Event(var evnt):
                            Console.WriteLine($"Received event {evnt.OriginalEventNumber}@{evnt.OriginalStreamId}");
                            HandleEvent(evnt);
                            UpdateCheckpoint(evnt.OriginalPosition!.Value);
                            break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"Subscription was canceled.");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine($"Subscription was canceled by the user.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Subscription was dropped: {ex}");
                await Subscribe(stoppingToken);
            }
        }

        internal override IPosition ReadStreamCheckpoint()
        {
            using var scope = serviceProvider.CreateScope();
            var checkpointRepository = scope.ServiceProvider.GetService<IAllCheckpointRepository>();

            var checkpoint = checkpointRepository!.GetSingle(GetType().Name);

            return checkpoint != null ? new Position(checkpoint.CommitPosition, checkpoint.PreparePosition) : null;
        }

        internal override void UpdateCheckpoint(IPosition eventNumber)
        {
            using var scope = serviceProvider.CreateScope();
            var checkpointRepository = scope.ServiceProvider.GetService<IAllCheckpointRepository>();

            var checkpoint = checkpointRepository!.GetSingle(GetType().Name);

            if (checkpoint != null)
            {
                checkpoint.SetPosition(((Position)eventNumber).CommitPosition, ((Position)eventNumber).PreparePosition);
                checkpointRepository.Update(checkpoint);
            }
            else
            {
                checkpoint = new AllCheckpoint
                {
                    Id = GetType().Name
                };

                checkpoint.SetPosition(((Position)eventNumber).CommitPosition, ((Position)eventNumber).PreparePosition);

                checkpointRepository.Add(checkpoint);
            }

        }
    }
}
