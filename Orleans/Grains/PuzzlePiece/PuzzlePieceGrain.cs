using Grains;
using Microsoft.Extensions.Logging;
using Orleans.Concurrency;
using Orleans.Providers;

namespace Orleans.Grains.PuzzlePiece;

[StorageProvider(ProviderName = "OrleansProvider")]
public class PuzzlePieceGrain : Grain<GrainState<Domain.PuzzlePieceAggregate.PuzzlePiece>>, IPuzzlePieceGrain
{
	private readonly ILogger _logger;

	public PuzzlePieceGrain(ILogger<PuzzlePieceGrain> logger)
	{
		_logger = logger;
	}

	public override async Task OnActivateAsync()
	{
		if (State.DomainAggregate == null)
		{
			State.DomainAggregate = new Domain.PuzzlePieceAggregate.PuzzlePiece();

			await WriteStateAsync();
		}
		await base.OnActivateAsync();
	}

	public Task Ping()
	{
		return Task.CompletedTask;
	}

	[AlwaysInterleave]
	public Task<string> GetBlockchainAssetId() => Task.FromResult(State.DomainAggregate!.BlockchainAssetId);

	[AlwaysInterleave]
	public Task<Guid> GetPuzzleDefinitionId() => Task.FromResult(State.DomainAggregate!.PuzzleDefinitionId);
}