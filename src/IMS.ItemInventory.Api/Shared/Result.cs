namespace IMS.ItemInventory.Api.Shared;

public class Result
{
    public int Temp { get; set; }
}

public class Result<TResponse>
{
    public int Temp { get; set; }
    public object? Value { get; internal set; }
}