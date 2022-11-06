using Orleans.Grains.ValueObjects;

namespace Domain.OrderAggregate;

public class Order
{
	public decimal PlatformFeeInLovelace { get; private set; }

	public decimal TransactionFeeInLovelace { get; private set; }

	public OrderState State { get; private set; }

	public BlockchainTransaction? BlockchainTransaction { get; private set; }

	public Guid UserId { get; private set; }

	private List<TradeInPuzzlePiece> _tradeInPuzzlePieces = new List<TradeInPuzzlePiece>();

	public IEnumerable<TradeInPuzzlePiece> TradeInPuzzlePieces
	{
		get { return _tradeInPuzzlePieces.AsReadOnly(); }
		private set { _tradeInPuzzlePieces = value.ToList(); }
	}

	private List<OrderedPuzzlePiece> _orderedPuzzlePieces = new List<OrderedPuzzlePiece>();

	public IEnumerable<OrderedPuzzlePiece> OrderedPuzzlePieces
	{
		get { return _orderedPuzzlePieces.AsReadOnly(); }
		private set { _orderedPuzzlePieces = value.ToList(); }
	}

	public void AddOrderedPuzzlePieceId(Guid puzzlePieceId, string blockchainAssetId, string policyId, decimal priceInLovelace)
	{
		if (_orderedPuzzlePieces.Select(o => o.Id).Contains(puzzlePieceId))
			return;
		_orderedPuzzlePieces.Add(new OrderedPuzzlePiece(puzzlePieceId, blockchainAssetId, policyId, priceInLovelace));
		CalculatePlatformFee();
	}

	public void AddTradeIdPuzzlePieceId(Guid puzzlePieceId, string blockchainAssetId, decimal tradeInValueInLovelace)
	{
		if (_tradeInPuzzlePieces.Select(o => o.Id).Contains(puzzlePieceId))
			return;
		_tradeInPuzzlePieces.Add(new TradeInPuzzlePiece(puzzlePieceId, blockchainAssetId, tradeInValueInLovelace));
		CalculatePlatformFee();
	}

	private void CalculatePlatformFee()
	{
		PlatformFeeInLovelace = 5_000_000;
	}

	public void SetOrderer(Guid userId)
	{
		UserId = userId;
	}

	public void CreateTransaction(IEnumerable<UtxoAsset> userWalletAvailableAssets)
	{
		BlockchainTransaction = new BlockchainTransaction();

		BlockchainTransaction.CreateTransaction(userWalletAvailableAssets);

		State = OrderState.TransactionCreated;
	}
}