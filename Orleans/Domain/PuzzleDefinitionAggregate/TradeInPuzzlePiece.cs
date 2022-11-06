namespace Domain.OrderAggregate;

public class TradeInPuzzlePiece
{
	public TradeInPuzzlePiece(Guid id, string blockchainAssetId, decimal tradeInValue)
	{
		Id = id;
		BlockchainAssetId = blockchainAssetId;
		TradeInValue = tradeInValue;
	}

	public Guid Id { get; private set; }

	public string BlockchainAssetId { get; private set; }

	public decimal TradeInValue { get; }
}