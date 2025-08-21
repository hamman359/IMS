namespace IMS.ItemInventory.Api.Shared.CRQS;

public interface ICommand : IBaseCommand { }

public interface ICommand<TResponse> : IBaseCommand { }

public interface IBaseCommand { }
