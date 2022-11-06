using Orleans.Grains.Order.Messages;

namespace Orleans.Grains.Order;

public interface IOrderGrain : Orleans.IGrainWithGuidKey
{
	Task<BuildOrderResponse> BuildOrder(BuildOrderCommand buildOrderCommand);
}