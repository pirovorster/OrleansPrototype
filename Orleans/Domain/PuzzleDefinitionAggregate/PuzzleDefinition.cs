namespace Domain.PuzzleDefinitionAggregate;

public class PuzzleDefinition
{
	public string PolicyId { get; private set; }

	public decimal PuzzlePiecePriceInLovelace { get; private set; }

	public decimal PuzzlePieceTradeInValueInLovelace { get; private set; }
}