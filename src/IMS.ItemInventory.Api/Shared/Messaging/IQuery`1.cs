using IMS.ItemInventory.Api.Shared.Results;

using MediatR;

namespace IMS.ItemInventory.Api.Shared.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
