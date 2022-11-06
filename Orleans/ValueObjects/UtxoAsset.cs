namespace Orleans.Grains.ValueObjects;

public record struct UtxoAsset(string TxId, int OutputIndexOnTx, string BlockchainAssetId)
{
	public Utxo GetUtxo() => new Utxo(TxId, OutputIndexOnTx);
}