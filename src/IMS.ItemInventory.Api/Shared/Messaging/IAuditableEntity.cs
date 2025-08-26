namespace IMS.ItemInventory.Api.Shared.Messaging;

public interface IAuditableEntity
{
    DateTime CreatedOnUtc { get; set; }
    DateTime? ModifiedOnUtc { get; set; }
}
