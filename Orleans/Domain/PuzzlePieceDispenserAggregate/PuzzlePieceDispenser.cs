using static System.FormattableString;

namespace Domain.PuzzlePieceDispenserAggregate;

public class PuzzlePieceDispenser
{
	public string Id { get; private set; }

	public Guid PuzzleCollectionId { get; private set; }

	public int PuzzleSize { get; private set; }

	public PuzzlePieceDispenser(Guid puzzleCollectionId, int puzzleSize)
	{
		PuzzleCollectionId = puzzleCollectionId;
		PuzzleSize = puzzleSize;
		Id = GetId(puzzleCollectionId, puzzleSize);
	}

	public static string GetId(Guid puzzleCollectionId, int puzzleSize) => Invariant($"{puzzleCollectionId}/{puzzleSize}");

	public static (Guid puzzleCollectionId, int puzzleSize) SplitId(string id)
	{
		var parts = id.Split("/");
		var puzzleCollectionId = Guid.Parse(parts[0]);
		var puzzleSize = int.Parse(parts[1]);

		return (puzzleCollectionId, puzzleSize);
	}

	public IEnumerable<Guid> AvailablePuzzlePieceIds
	{
		get { return _availablePuzzlePieceIds.AsReadOnly(); }
		private set { _availablePuzzlePieceIds = value.ToList(); }
	}

	private List<Guid> _availablePuzzlePieceIds = new List<Guid>();

	public IEnumerable<Guid> ReservedPuzzlePieceIds
	{
		get { return _reservedPuzzlePieceIds.AsReadOnly(); }
		private set { _reservedPuzzlePieceIds = value.ToList(); }
	}

	private List<Guid> _reservedPuzzlePieceIds = new List<Guid>();

	public void AddAvailablePuzzlePiece(Guid puzzlePieceId)
	{
		_availablePuzzlePieceIds.Add(puzzlePieceId);
	}

	public IEnumerable<Guid> ReserveRandomPuzzlePieces(int count)
	{
		var reservations = _availablePuzzlePieceIds.Except(_reservedPuzzlePieceIds)
			.OrderBy(o => Guid.NewGuid())
			.Take(count);

		AddReservation(reservations);

		//TODO: what if too little reservation available. Thought here is that one returns as many as possible and let the user know.
		return reservations;
	}

	private void AddReservation(IEnumerable<Guid> reservations)
	{
		_reservedPuzzlePieceIds.AddRange(reservations);
	}
}