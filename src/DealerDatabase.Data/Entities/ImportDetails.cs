namespace DealerDatabase.Data.Entities;

public sealed record ImportDetails : Base.Entity
{
	public required DateTime RanAt { get; set; }

	public DateTime? CompletedAt { get; set; }

	public int RecordsCreated { get; set; }
}