using IMS.ItemInventory.Api.Data;
using IMS.ItemInventory.Api.Shared.Messaging;
using IMS.SharedKernal.Outbox;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using Polly;
using Polly.Retry;

using Quartz;

namespace IMS.ItemInventory.Api.BackgroundJobs;

[DisallowConcurrentExecution] //Ensures only one instance of the background job can run at a time
public class ProcessOutboxMessagesJob(
    InventoryManagementDbContext dbContext,
    IPublisher publisher)
    : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        // Load a batch of unprocessed messages from the DB.
        // The size of the batch of messages is kept relatively low (20)
        // so that the job completes fairly quickly. Since the job
        // is schedule to run continuously, all messages will eventually be processed.
        var messages = await dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach (var outboxMessage in messages)
        {
            var domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

            if (domainEvent is null)
            {
                //TO DO: Add logging. domainEvent shouldn't ever be null here
                continue;
            }

            // Setting up a retry policy with Polly so that multiple attempts
            // are made to handle each message. This should help prevent the
            // handling of the job from failing due to a temporary or intermittent
            // issue. For more on doing retries with Polly: https://www.pollydocs.org/
            var pipeline = new ResiliencePipelineBuilder()
                .AddRetry(new RetryStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                    Delay = TimeSpan.FromSeconds(1),
                    MaxRetryAttempts = 3,
                    BackoffType = DelayBackoffType.Exponential
                })
                .Build();


            await pipeline.ExecuteAsync(
                async token =>
                        await publisher.Publish(
                            domainEvent,
                            context.CancellationToken));

            // Set the time the message was processed to mark it as processed.
            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }

        //Save the updated OutboxMessage to the DB
        await dbContext.SaveChangesAsync();
    }
}