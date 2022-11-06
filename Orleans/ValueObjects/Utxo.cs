namespace Orleans.Grains.ValueObjects;

public record struct Utxo(string TxId, int OutputIndexOnTx)
{
	public override string ToString() => $"({TxId}:{OutputIndexOnTx})";
}