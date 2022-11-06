namespace Orleans;

public class TestData
{
	public class TestPuzzleDefinition
	{
		public List<Guid> PuzzlePieceIds { get; set; } = new List<Guid>();

		public Guid Id { get; set; }

		public int Size { get; set; }
	}

	public static List<TestPuzzleDefinition> GetPuzzleDefinitions()
	{
		List<TestPuzzleDefinition> list = new List<TestPuzzleDefinition>();
		string stringId = "00000000-0000-0000-0000-000000000000";

		return Enumerable.Range(1, 1000)
			.Select(o => o.ToString())
			.Select(o =>
			Guid.Parse($"{o}{stringId.Substring(o.Length, stringId.Length - o.Length)}"))
			.Select(o => CreateTestPuzzleDefinition(o))
			.ToList();
	}

	public static List<Guid> GetUserWalletIds()
	{
		return Enumerable.Range(1, 100)
			.Select(o => Guid.NewGuid())
			.ToList();
	}

	private static TestPuzzleDefinition CreateTestPuzzleDefinition(Guid id)
	{
		string stringId = id.ToString();
		return new TestPuzzleDefinition
		{
			Id = id,
			Size = 5,
			PuzzlePieceIds = Enumerable.Range(1, 15)
				.Select(o => o.ToString())
				.Select(o => Guid.Parse($"{stringId.Substring(0, stringId.Length - o.Length)}{o}"))
				.ToList()
		};
	}
}