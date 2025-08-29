using Carter;

using MediatR;

namespace IMS.ItemInventory.Api.Features;

public static class TestFeature
{
    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/test", async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new TestQuery();

                var result = await sender.Send(query, cancellationToken);

                return Results.Ok(result);
            })
            .WithName("Test");
        }
    }
}
