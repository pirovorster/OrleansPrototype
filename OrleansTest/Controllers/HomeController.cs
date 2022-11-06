using Microsoft.AspNetCore.Mvc;
using Orleans;
using Orleans.Grains.PuzzleDefinition;
using Orleans.Grains.PuzzlePiece;
using Orleans.Grains.Store;
using Orleans.Grains.UserWallet;
using OrleansTest.Models;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace OrleansTest.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private readonly IClusterClient _clusterClient;

	public HomeController(ILogger<HomeController> logger, IClusterClient clusterClient)
	{
		_logger = logger;
		_clusterClient = clusterClient;
	}

	[HttpGet]
	public async Task<IActionResult> Index()
	{
		return View();
	}

	[HttpGet]
	public async Task<IActionResult> WakeUp()
	{
		var userWalletIds = TestData.GetUserWalletIds();
		var testDefinitions = TestData.GetPuzzleDefinitions();

		foreach (var id in userWalletIds)
		{
			var walletGrain = _clusterClient.GetGrain<IUserWalletGrain>(id);
			await walletGrain.Ping(); //make sure grain is loaded
		}
		await Parallel.ForEachAsync(testDefinitions, async (definition, _) =>
		{
			await _clusterClient.GetGrain<IPuzzleDefinitionGrain>(definition.Id).Ping();
			await _clusterClient.GetGrain<IPuzzlePieceDispenserGrain>($"{definition.Id}/{definition.Size}").AddStock(definition.PuzzlePieceIds);
			foreach (var puzzlePieceId in definition.PuzzlePieceIds)
			{
				await _clusterClient.GetGrain<IPuzzlePieceGrain>(puzzlePieceId).Ping();
			}
		});

		return Content("I'm awake");
	}

	[HttpGet]
	public IActionResult Order()
	{
		ConcurrentQueue<Task> tasks = new ConcurrentQueue<Task>();

		var userWalletIds = TestData.GetUserWalletIds();
		var testDefinitions = TestData.GetPuzzleDefinitions();
		var tradeInDefinition = testDefinitions.Take(1).Single();
		testDefinitions = testDefinitions.Skip(1).ToList();

		var sw = Stopwatch.StartNew();
		Parallel.ForEach(Enumerable.Range(1, 10000), (o) =>
		{
			var definition = testDefinitions[o % testDefinitions.Count];
			var command = new Orleans.Grains.UserWallet.Messages.CreateOrderCommand(definition.Id, definition.Size, 1);
			var userWalletGrain = _clusterClient.GetGrain<IUserWalletGrain>(userWalletIds[o % userWalletIds.Count]);

			tasks.Enqueue(userWalletGrain.CreateOrder(command));
		});

		Task.WaitAll(tasks.ToArray());
		return Json(new { Elapsede = sw.ElapsedMilliseconds });
	}

	public IActionResult Privacy()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}