namespace IMS.ItemInventory.Api.Shared.CRQS;

public interface ICommandHandler<in TCommand>
    : IBaseCommandHandler
    where TCommand : ICommand
{
    Task<Result> Handle(TCommand command, CancellationToken cancelationToken);
}

public interface ICommandHandler<in TCommand, TResponse>
    : IBaseCommandHandler
    where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancelationToken);
}

public interface IBaseCommandHandler { }
