namespace IMS.SharedKernal.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
