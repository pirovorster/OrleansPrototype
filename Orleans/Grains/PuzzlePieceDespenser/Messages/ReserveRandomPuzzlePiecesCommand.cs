namespace Orleans.Grains.PuzzlePieceDispenser.Messages;

public record ReserveRandomPuzzlePiecesCommand(int Quantity)
{
}

public record ReserveRandomPuzzlePiecesResponse(List<Guid> DispensedPuzzlePieceIds)
{
	public List<Guid> DispensedPuzzlePieceIds { get; set; } = DispensedPuzzlePieceIds ?? new List<Guid>();
}