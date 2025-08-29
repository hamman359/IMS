using IMS.ItemInventory.Api.Data;
using IMS.ItemInventory.Api.Shared.Messaging;
using IMS.ItemInventory.Api.Shared.Outbox;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using Polly;
using Polly.Retry;

using Quartz; //Use another scheduler?

namespace IMS.ItemInventory.Api.Shared.BackgroundJobs;

[DisallowConcurrentExecution] //Ensures only one instance of the background job can run at a time
public class ProcessOutboxMessagesJob(
    InventoryManagementDbContext dbContext,
    IPublisher publisher)
    : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        //TODO Add exception Handling
        List<OutboxMessage> messages = await dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach (var outboxMessage in messages)
        {
            IDomainEvent? domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

            if (domainEvent is null)
            {
                //TODO: Add logging. domainEvent shouldn't ever be null here
                continue;
            }

            //TODO: Handle exceptions?
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

            //outboxMessage.Error = pipelineResult.FinalException?.ToString();
            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync();
    }
}