using Orleans.Grains.UserWallet.Messages;

namespace Orleans.Grains.UserWallet;

public interface IUserWalletGrain : Orleans.IGrainWithGuidKey
{
	Task<CreateOrderResponse> CreateOrder(CreateOrderCommand command);

	Task Ping();
}