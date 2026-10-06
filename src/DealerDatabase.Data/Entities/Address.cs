using System.ComponentModel.DataAnnotations;

namespace DealerDatabase.Data.Entities;

public sealed record Address : Base.Entity
{
	// [MaxLength(2)]
	// public required string CountryCode { get; set; }

	[MaxLength(50)]
	public string? Country { get; set; }

	[MaxLength(50)]
	public string? County { get; set; }

	[MaxLength(10)]
	public string? PostalCode { get; set; }

	[MaxLength(60)]
	public string? Town { get; set; }

	[MaxLength(100)]
	public string? Line1 { get; set; }

	[MaxLength(100)]
	public string? Line2 { get; set; } = "";

	[MaxLength(100)]
	public string? Line3 { get; set; } = "";
}