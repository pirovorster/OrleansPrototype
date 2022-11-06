using Orleans.Grains.ValueObjects;

namespace Orleans.Grains.Order.Messages;

public record BuildOrderCommand(Guid UserId, Guid PuzzleCollectionId, int PuzzleSize, List<Guid> OrderedPuzzlePieceIds, List<UtxoAsset> UserWalletAssets)
{
	public List<Guid> OrderedPuzzlePieceIds { get; set; } = OrderedPuzzlePieceIds ?? new List<Guid>();
	public List<UtxoAsset> UserWalletAssets { get; set; } = UserWalletAssets?? new List<UtxoAsset>();
}

public record BuildOrderResponse(List<Utxo> UsedUtxos)
{
	public List<Utxo> UsedUtxos { get; set; } = UsedUtxos?? new List<Utxo>();
};