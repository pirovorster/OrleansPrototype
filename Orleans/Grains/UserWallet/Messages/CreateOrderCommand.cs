namespace Orleans.Grains.UserWallet.Messages;

public record CreateOrderCommand(Guid PuzzleCollectionId, int PuzzleSize, int Quantity);

public record CreateOrderResponse(Guid orderId);