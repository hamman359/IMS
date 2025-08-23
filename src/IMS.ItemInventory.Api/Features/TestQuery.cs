
using IMS.ItemInventory.Api.Shared;
using IMS.ItemInventory.Api.Shared.CRQS;

namespace IMS.ItemInventory.Api.Features;

public class TestQuery : IQuery<string>
{
}

public class TestQueryHandler : IQueryHandler<TestQuery, string>
{
    public async Task<Result<string>> Handle(TestQuery query, CancellationToken cancelationToken)
    {
        await Task.CompletedTask;

        return "";
    }

}
