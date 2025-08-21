using Carter;

using IMS.ItemInventory.Api.Shared.CRQS;

namespace IMS.ItemInventory.Api.Features;

public static class TestFeature
{
    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/test", async (
                IDispatcher dispatcher,
                CancellationToken cancellationToken) =>
            {
                var query = new TestQuery();

                var result = await dispatcher.QueryAsync(query, cancellationToken);

                return Results.Ok(result);
            })
            .WithName("Test");
        }
    }
}
