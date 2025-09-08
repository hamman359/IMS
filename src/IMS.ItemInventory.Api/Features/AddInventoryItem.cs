using Carter;

using FluentValidation;

using IMS.ItemInventory.Api.Data.Repositories;
using IMS.ItemInventory.Api.Model.Entities;

namespace IMS.ItemInventory.Api.Features;

internal static class AddInventoryItem
{
    internal sealed class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/InventoryItem", async (
                Command command,
                ISender sender,
                CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(command, cancellationToken);

                    return result;
                })
                .WithName("Add Inventory Item");
        }
    }

    internal sealed record Command(
        string Sku,
        string Name,
        string Description)
        : ICommand<Guid>;

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Sku).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }

    internal sealed class CommandHandler(
       IInventoryItemRepository repository)
       : ICommandHandler<Command, Guid>
    {
        public Task<Result<Guid>> Handle(
            Command command,
            CancellationToken cancellationToken)
        {
            var item = InventoryItem.Create(
                command.Sku,
                command.Name,
                command.Description);

            if (item.IsFailure)
            {
                return Task.FromResult(Result.Failure<Guid>(item.Errors));
            }

            repository.Add(item.Value);

            return Task.FromResult(Result.Success(item.Value.Id));
        }
    }
}