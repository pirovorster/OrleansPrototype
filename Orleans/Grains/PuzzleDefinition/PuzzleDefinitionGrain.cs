using Grains;
using Microsoft.Extensions.Logging;
using Orleans.Concurrency;
using Orleans.Grains.PuzzlePiece;
using Orleans.Providers;

namespace Orleans.Grains.PuzzleDefinition;

[StorageProvider(ProviderName = "OrleansProvider")]
public class PuzzleDefinitionGrain : Grain<GrainState<Domain.PuzzleDefinitionAggregate.PuzzleDefinition>>, IPuzzleDefinitionGrain
{
	private readonly ILogger _logger;

	public PuzzleDefinitionGrain(ILogger<PuzzlePieceGrain> logger)
	{
		_logger = logger;
	}

	public override async Task OnActivateAsync()
	{
		if (State.DomainAggregate == null)
		{
			State.DomainAggregate = new Domain.PuzzleDefinitionAggregate.PuzzleDefinition();

			await WriteStateAsync();
		}
		await base.OnActivateAsync();
	}

	public Task Ping()
	{
		return Task.CompletedTask;
	}

	[AlwaysInterleave]
	public Task<string> GetPolicyId() => Task.FromResult(State.DomainAggregate!.PolicyId);

	[AlwaysInterleave]
	public Task<decimal> GetPuzzlePiecePriceInLovelace() => Task.FromResult(State.DomainAggregate!.PuzzlePiecePriceInLovelace);

	[AlwaysInterleave]
	public Task<decimal> GetPuzzlePieceTradeInValueInLovelace() => Task.FromResult(State.DomainAggregate!.PuzzlePieceTradeInValueInLovelace);
}