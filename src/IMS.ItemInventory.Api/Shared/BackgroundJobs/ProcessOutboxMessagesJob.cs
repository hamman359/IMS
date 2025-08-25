using IMS.ItemInventory.Api.Data;
using IMS.ItemInventory.Api.Shared.Messaging;
using IMS.ItemInventory.Api.Shared.Outbox;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using Polly;
using Polly.Retry;

using Quartz; //Use another scheduler?

namespace IMS.ItemInventory.Api.Shared.BackgroundJobs;

[DisallowConcurrentExecution] //Ensures only one instance of the background job can run at a time
public class ProcessOutboxMessagesJob : IJob
{
    private readonly InventoryManagementDbContext _dbContext;
    private readonly IDispatcher _dispatcher; //Need replacement for MediatR IDispatcher

    public ProcessOutboxMessagesJob(InventoryManagementDbContext dbContext, IDispatcher dispatcher)
    {
        _dbContext = dbContext;
        _dispatcher = dispatcher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        //TODO Add exception Handling
        List<OutboxMessage> messages = await _dbContext
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
                continue;
            }

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
                        await _dispatcher.SendDomainEventAsync<IDomainEvent>(
                            domainEvent,
                            context.CancellationToken));

            //outboxMessage.Error = pipelineResult.FinalException?.ToString();
            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();
    }
}