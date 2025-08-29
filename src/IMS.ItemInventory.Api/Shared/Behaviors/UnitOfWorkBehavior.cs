
using System.Transactions;
using System.Windows.Input;

using IMS.ItemInventory.Api.Shared.Persistence;

using MediatR;

namespace IMS.ItemInventory.Api.Shared.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (IsNotCommand())
        {
            return await next(cancellationToken);
        }

        using var transactionScope = new TransactionScope();

        var response = await next(cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        transactionScope.Complete();

        return response;
    }

    private static bool IsNotCommand() =>
        !typeof(TRequest).Equals(typeof(ICommand));
}
