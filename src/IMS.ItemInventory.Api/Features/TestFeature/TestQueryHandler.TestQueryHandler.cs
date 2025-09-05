namespace IMS.ItemInventory.Api.Features.TestFeature;

internal class TestQueryHandler
    : IQueryHandler<TestQuery, string>
{
    public async Task<Result<string>> Handle(TestQuery query, CancellationToken cancelationToken)
    {
        await Task.CompletedTask;

        return Result.Success("");
    }
}
