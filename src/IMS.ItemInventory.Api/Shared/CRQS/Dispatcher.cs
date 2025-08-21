namespace IMS.ItemInventory.Api.Shared.CRQS;

public class Dispatcher(IServiceProvider provider) : IDispatcher
{
    private readonly IServiceProvider _provider = provider;

    public Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
        dynamic handler = _provider.GetRequiredService(handlerType);

        if (handler is null)
        {
            throw new ArgumentException("");
        }

        return handler.Handle((dynamic)query, cancellationToken);
    }

    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType());
        dynamic handler = _provider.GetRequiredService(handlerType);

        if (handler is null)
        {
            throw new ArgumentException("");
        }

        return handler.Handle((dynamic)command, cancellationToken);
    }
}