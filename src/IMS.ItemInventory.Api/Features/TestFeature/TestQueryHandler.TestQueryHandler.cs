namespace IMS.ItemInventory.Api.Features.TestFeature;

internal class TestQueryHandler
    : IQueryHandler<TestQuery, string>
{
    public async Task<Result<string>> Handle(TestQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        return Result.Success("");
    }
}
