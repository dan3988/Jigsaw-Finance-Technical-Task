namespace DealerDatabase.Data.Entities;

public sealed record DealerTradingName : Base.Entity
{
	public int DealerId { get; set; }

	public Dealer Dealer
	{
		get => field ?? throw new UninitializedNavigationPropertyException(GetType());
		set;
	}

	public required string Name { get; set; }
}