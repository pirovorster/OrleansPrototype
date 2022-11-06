namespace Orleans.Grains.PuzzleDefinition;

public interface IPuzzleDefinitionGrain : Orleans.IGrainWithGuidKey
{
	public Task<string> GetPolicyId();

	public Task<decimal> GetPuzzlePiecePriceInLovelace();

	public Task<decimal> GetPuzzlePieceTradeInValueInLovelace();

	Task Ping();
}