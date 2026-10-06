namespace DealerDatabase.Data.Entities;

public sealed record DealerOfficer : Base.Entity
{
	public int DealerId { get; set; }

	public Dealer Dealer
	{
		get => field ?? throw new UninitializedNavigationPropertyException(GetType());
		set;
	}

	public required string Name { get; set; }

	public required OfficerRole OfficerRole { get; set; }

	public required DateOnly AppointedOn { get; set; }

	public DateOnly? ResignedOn { get; set; }

	public string? Nationality { get; set; }

	public string? Occupation { get; set; }
}