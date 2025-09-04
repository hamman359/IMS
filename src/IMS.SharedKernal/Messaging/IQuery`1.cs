using IMS.SharedKernal.Results;

using MediatR;

namespace IMS.SharedKernal.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
