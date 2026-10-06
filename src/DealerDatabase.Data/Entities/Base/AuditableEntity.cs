namespace DealerDatabase.Data.Entities.Base;

public abstract record AuditableEntity : Entity
{
	public required DateTime CreatedAt { get; set; }

	public DateTime? UpdatedAt { get; set; }
}