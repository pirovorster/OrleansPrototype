using Orleans.Grains.PuzzlePieceDispenser.Messages;

namespace Orleans.Grains.Store;

public interface IPuzzlePieceDispenserGrain : Orleans.IGrainWithStringKey
{
	Task<ReserveRandomPuzzlePiecesResponse> ReserveRandomPuzzlePieces(ReserveRandomPuzzlePiecesCommand command);

	Task AddStock(IEnumerable<Guid> puzzlePieceIds);
}