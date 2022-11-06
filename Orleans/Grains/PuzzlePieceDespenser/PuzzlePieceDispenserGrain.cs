using Grains;
using Microsoft.Extensions.Logging;
using Orleans.Grains.PuzzlePieceDispenser.Messages;
using Orleans.Grains.Store;
using Orleans.Providers;

namespace Orleans.Grains.PuzzlePieceDespenser;

[StorageProvider(ProviderName = "OrleansProvider")]
public class PuzzlePieceDispenserGrain : Grain<GrainState<Domain.PuzzlePieceDispenserAggregate.PuzzlePieceDispenser>>, IPuzzlePieceDispenserGrain
{
	private readonly ILogger _logger;

	public PuzzlePieceDispenserGrain(ILogger<PuzzlePieceDispenserGrain> logger)
	{
		_logger = logger;
	}

	public override async Task OnActivateAsync()
	{
		if (State.DomainAggregate == null)
		{
			(Guid puzzleCollectionId, int puzzleSize) = Domain.PuzzlePieceDispenserAggregate.PuzzlePieceDispenser.SplitId(this.GetGrainIdentity().PrimaryKeyString);

			State.DomainAggregate = new Domain.PuzzlePieceDispenserAggregate.PuzzlePieceDispenser(puzzleCollectionId, puzzleSize);

			await WriteStateAsync();
		}
		await base.OnActivateAsync();
	}

	public async Task AddStock(IEnumerable<Guid> puzzlePieceIds)
	{
		foreach (var puzzlePieceId in puzzlePieceIds)
		{
			State.DomainAggregate!.AddAvailablePuzzlePiece(puzzlePieceId);
		}

		await WriteStateAsync();
	}

	public async Task<ReserveRandomPuzzlePiecesResponse> ReserveRandomPuzzlePieces(ReserveRandomPuzzlePiecesCommand command)
	{
		var reservedPuzzlePieceIds = State.DomainAggregate!.ReserveRandomPuzzlePieces(command.Quantity);

		await WriteStateAsync();

		return new ReserveRandomPuzzlePiecesResponse(reservedPuzzlePieceIds.ToList());
	}
}