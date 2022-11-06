namespace Orleans.Grains.PuzzlePiece;

public interface IPuzzlePieceGrain : Orleans.IGrainWithGuidKey
{
	public Task<string> GetBlockchainAssetId();

	public Task<Guid> GetPuzzleDefinitionId();

	Task Ping();
}