namespace IMS.ItemInventory.Api.Configuration.Options;

internal class DatabaseOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public int MaxRetryCount { get; set; }
    public int CommandTimeout { get; set; }
    public bool EnableDetailedErrors { get; set; }
    // Should only be turned on in DEV environments because it can expose sensitive data
    public bool EnableSensitiveDataLogging { get; set; }
}
