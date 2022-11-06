using Orleans.Grains.ValueObjects;

namespace Domain.UserWalletAggregate;

public class UserWallet
{
	public Guid Id { get; private set; }

	public Guid ActiveOrderId { get; private set; }

	private List<UtxoReservation> _utxoReservations = new List<UtxoReservation>();

	public IEnumerable<UtxoReservation> UtxoReservations
	{
		get { return _utxoReservations.AsReadOnly(); }
		private set { _utxoReservations = value.ToList(); }
	}

	public IEnumerable<Utxo> ReservedUtxos => UtxoReservations.Select(o => o.Utxo);

	public void SetActiveOrder(Guid orderId, IEnumerable<Utxo> utxosUsed)
	{
		_utxoReservations.RemoveAll(o => o.Reserver == Reserver.Order && o.ReserverId == ActiveOrderId);

		var reservationConflicts = ReservedUtxos.Intersect(utxosUsed).ToList();
		if (reservationConflicts.Count > 0)
			throw new Exception($"The following utxos have already been reserved: {string.Join(",", reservationConflicts.Select(o => o.ToString()))}");

		_utxoReservations.AddRange(utxosUsed.Select(o => new UtxoReservation(o, Reserver.Order, orderId)));

		ActiveOrderId = orderId;
	}
}