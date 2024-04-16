
using System;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using illyW.Framework.EventStore.Entities;
using illyW.Framework.EventStore.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace illyW.Framework.EventStore.Rehydrators
{
    public abstract class StreamRehydrator(IServiceProvider serviceProvider, EventStoreClient client, string streamName, ILogger logger) : BaseRehydrator
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
                null => FromStream.Start,
                var position => FromStream.After((StreamPosition)position)
            };

            try
            {
                var subscription = client.SubscribeToStream(streamName, checkpoint, cancellationToken: stoppingToken);

                await foreach (var message in subscription.Messages)
                {
                    switch (message)
                    {
                        case StreamMessage.Event(var evnt):
                            logger.LogDebug($"Received event {evnt.OriginalEventNumber}@{evnt.OriginalStreamId}");
                            HandleEvent(evnt);
                            UpdateCheckpoint(evnt.OriginalEventNumber);
                            break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogError($"Subscription was canceled.");
            }
            catch (ObjectDisposedException)
            {
                logger.LogError($"Subscription was canceled by the user.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Subscription was dropped: {ex}");
                await Subscribe(stoppingToken);
            }
        }

        internal override IPosition ReadStreamCheckpoint()
        {
            using var scope = serviceProvider.CreateScope();
            var checkpointRepository = scope.ServiceProvider.GetService<IStreamCheckpointRepository>();

            var checkpoint = checkpointRepository!.GetSingle(GetType().Name);

            return checkpoint != null ? StreamPosition.FromInt64((long)checkpoint.CommitPosition) : null;
        }

        internal override void UpdateCheckpoint(IPosition eventNumber)
        {
            using var scope = serviceProvider.CreateScope();
            var checkpointRepository = scope.ServiceProvider.GetService<IStreamCheckpointRepository>();

            var checkpoint = checkpointRepository!.GetSingle(GetType().Name);

            if (checkpoint != null)
            {
                checkpoint.SetCommitPosition(((StreamPosition)eventNumber).ToUInt64());
                checkpointRepository.Update(checkpoint);
            }
            else
            {
                checkpoint = new StreamCheckpoint
                {
                    Id = GetType().Name
                };

                checkpoint.SetCommitPosition(((StreamPosition)eventNumber).ToUInt64());
                checkpointRepository.Add(checkpoint);
            }

        }
    }
}
