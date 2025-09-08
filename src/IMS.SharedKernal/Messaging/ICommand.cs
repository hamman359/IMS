namespace IMS.SharedKernal.Messaging;

public interface ICommand : IRequest<Result> { }

public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }

