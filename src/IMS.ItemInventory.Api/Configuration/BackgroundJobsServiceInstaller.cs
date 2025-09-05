using IMS.ItemInventory.Api.BackgroundJobs;
using IMS.SharedKernal.Configuration;

using Quartz;

namespace IMS.ItemInventory.Api.Configuration;

internal sealed class BackgroundJobsServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        // Registers a background job that reads messages from the DB and
        // executes any Handlers that handle that message
        services.AddScoped<IJob, ProcessOutboxMessagesJob>();

        // Configures the Quartz service that runs the job above.
        // Additional information available at https://www.quartz-scheduler.net/
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(
                    trigger =>
                        trigger.ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule =>
                                    schedule.WithIntervalInSeconds(100)
                                        .RepeatForever()));
        });

        services.AddQuartzHostedService();
    }
}