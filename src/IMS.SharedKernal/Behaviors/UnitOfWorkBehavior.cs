using System.Transactions;
using System.Windows.Input;

using IMS.SharedKernal.Persistence;

namespace IMS.SharedKernal.Behaviors;

/// <summary>
/// Defines a MediatR pipeline behavior that automatically wraps all commands
/// in a transaction and eliminates the need to remember to explicitly call SaveChanges
/// in every Command.
/// This approach only works when using EF Core. An ORM that does not have Track Changes
/// behavior would not be able to be implemented this way.
/// </summary>
public sealed class UnitOfWorkBehavior<TRequest, TResponse>
    (IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // The TransactionScope and SaveChanges only apply to calls to commands, so if this
        // isn't a command we don't need to do anything here and can just forward the call
        if (IsNotCommand())
        {
            return await next(cancellationToken);
        }

        using var transactionScope = new TransactionScope();

        var response = await next(cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        transactionScope.Complete();

        return response;
    }

    private static bool IsNotCommand() =>
        !typeof(TRequest).Equals(typeof(ICommand));
}
