namespace IMS.ItemInventory.Api.Shared.Results;

/// <summary>
/// Represents the Validation result being returned by the Validation Pipeline Behavior
/// </summary>
public interface IValidationResult
{
    public static readonly Error ValidationError = new(
        "ValidationError",
        "A validation problem occurred.");

    Error[] Errors { get; }
}