using Grains;
using Microsoft.Extensions.Logging;
using Orleans.Grains.Order;
using Orleans.Grains.Order.Messages;
using Orleans.Grains.Store;
using Orleans.Grains.UserWallet.Messages;
using Orleans.Grains.ValueObjects;
using Orleans.Providers;

namespace Orleans.Grains.UserWallet;

[StorageProvider(ProviderName = "OrleansProvider")]
public class UserWalletGrain : Grain<GrainState<Domain.UserWalletAggregate.UserWallet>>, IUserWalletGrain
{
	private readonly ILogger _logger;

	public UserWalletGrain(ILogger<UserWalletGrain> logger)
	{
		_logger = logger;
	}

	public override async Task OnActivateAsync()
	{
		if (State.DomainAggregate == null)
		{
			State.DomainAggregate = new Domain.UserWalletAggregate.UserWallet();

			await WriteStateAsync();
		}
		await base.OnActivateAsync();
	}

	public async Task<CreateOrderResponse> CreateOrder(CreateOrderCommand command)
	{
		if (State.DomainAggregate == null)
			throw new Exception("UserWallet does not exist yet");

		var dispenserGrain = GrainFactory
			.GetGrain<IPuzzlePieceDispenserGrain>(Domain.PuzzlePieceDispenserAggregate.PuzzlePieceDispenser.GetId(command.PuzzleCollectionId, command.PuzzleSize));

		var reservationResponse = await dispenserGrain
			.ReserveRandomPuzzlePieces(new Orleans.Grains.PuzzlePieceDispenser.Messages.ReserveRandomPuzzlePiecesCommand(command.Quantity));

		var orderId = Guid.NewGuid();
		var orderGrain = GrainFactory
			.GetGrain<IOrderGrain>(orderId);

		var buildOrderCommand = new BuildOrderCommand(this.GetPrimaryKey(), command.PuzzleCollectionId, command.PuzzleSize, reservationResponse.DispensedPuzzlePieceIds.ToList(), new List<UtxoAsset>());

		var buildOrderResponse = await orderGrain.BuildOrder(buildOrderCommand);

		State.DomainAggregate!.SetActiveOrder(orderId, buildOrderResponse.UsedUtxos);

		await WriteStateAsync();

		return new CreateOrderResponse(Guid.NewGuid());
	}

	public Task Ping()
	{
		return Task.CompletedTask;
	}
}