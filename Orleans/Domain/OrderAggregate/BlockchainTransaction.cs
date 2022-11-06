using Orleans.Grains.ValueObjects;

namespace Domain.OrderAggregate;

public class BlockchainTransaction
{
	public Guid Id { get; private set; }

	public string? CborHex { get; private set; }

	private List<UtxoAsset> _userWalletInputBlockchainAssets = new List<UtxoAsset>();

	public IEnumerable<UtxoAsset> UserWalletInputBlockchainAssets
	{
		get { return _userWalletInputBlockchainAssets.AsReadOnly(); }
		private set { _userWalletInputBlockchainAssets = value.ToList(); }
	}

	public void CreateTransaction(IEnumerable<UtxoAsset> userWalletAvailableAssets)
	{
		_userWalletInputBlockchainAssets.AddRange(userWalletAvailableAssets.Take(2).ToList());
		CborHex = Guid.NewGuid().ToString();
	}
}