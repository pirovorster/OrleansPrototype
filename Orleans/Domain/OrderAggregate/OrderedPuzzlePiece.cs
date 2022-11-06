namespace Domain.OrderAggregate;

public class OrderedPuzzlePiece
{
	public OrderedPuzzlePiece(Guid id, string blockchainAssetId, string policyId, decimal priceInLovelace)
	{
		Id = id;
		BlockchainAssetId = blockchainAssetId;
		PolicyId = policyId;
		PriceInLovelace = priceInLovelace;
	}

	public Guid Id { get; private set; }

	public string BlockchainAssetId { get; private set; }

	public string PolicyId { get; private set; }

	public decimal PriceInLovelace { get; }
}