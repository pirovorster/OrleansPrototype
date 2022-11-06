namespace Domain.OrderAggregate;

public enum OrderState
{
	Draft = 0,
	TransactionCreated = 1,
	TransactionSigned = 2,
	TransactionSubmitted = 3,
	Completed = 4
}

[Flags]
public enum SignedState
{
	None = 0,
	SystemSigned = 1,
	UserSigned = 2
}