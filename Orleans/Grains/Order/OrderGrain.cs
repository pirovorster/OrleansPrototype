using Grains;
using Microsoft.Extensions.Logging;
using Orleans.Grains.Order.Messages;
using Orleans.Grains.PuzzleDefinition;
using Orleans.Grains.PuzzlePiece;
using Orleans.Providers;

namespace Orleans.Grains.Order;

[StorageProvider(ProviderName = "OrleansProvider")]
public class OrderGrain : Grain<GrainState<Domain.OrderAggregate.Order>>, IOrderGrain
{
	private readonly ILogger _logger;

	public OrderGrain(ILogger<OrderGrain> logger)
	{
		_logger = logger;
	}

	public async Task<BuildOrderResponse> BuildOrder(BuildOrderCommand buildOrderCommand)
	{
		if (State.DomainAggregate != null)
			throw new Exception("Order has already bean built");

		State.DomainAggregate = new Domain.OrderAggregate.Order();
		var order = State.DomainAggregate;

		foreach (var id in buildOrderCommand.OrderedPuzzlePieceIds)
		{
			var puzzlePieceGrain = GrainFactory.GetGrain<IPuzzlePieceGrain>(id);
			var puzzlePieceDefinitionId = await puzzlePieceGrain.GetPuzzleDefinitionId();
			var blockchainAssetId = await puzzlePieceGrain.GetBlockchainAssetId();
			var puzzlePieceDefinitionGrain = GrainFactory.GetGrain<IPuzzleDefinitionGrain>(puzzlePieceDefinitionId);
			var policyId = await puzzlePieceDefinitionGrain.GetPolicyId();
			var puzzlePieceTradeInValueInLovelace = await puzzlePieceDefinitionGrain.GetPuzzlePieceTradeInValueInLovelace();
			var puzzlePiecePriceInLovelace = await puzzlePieceDefinitionGrain.GetPuzzlePiecePriceInLovelace();

			order.AddOrderedPuzzlePieceId(id, blockchainAssetId, policyId, puzzlePiecePriceInLovelace);
		}

		order.SetOrderer(buildOrderCommand.UserId);

		order.CreateTransaction(buildOrderCommand.UserWalletAssets);

		await WriteStateAsync();

		var usedUtxos = order.BlockchainTransaction!.UserWalletInputBlockchainAssets
			.Select(o => o.GetUtxo())
			.Distinct()
			.ToList();

		return new BuildOrderResponse(usedUtxos);
	}
}