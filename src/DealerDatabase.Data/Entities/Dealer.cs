using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealerDatabase.Data.Entities;

/// <summary>
/// A consolidated dealership record.
/// This is a minimal starting point - extend or replace it as you see fit.
/// </summary>
public record Dealer : Base.AuditableEntity
{
	public required string NormalizedName { get; set; }

	public string? LegalName { get; set; }

	public DateOnly? IncorporationDate { get; set; }

	public string? PhoneNumber { get; set; }

	public string? Website { get; set; }

	public string? Email { get; set; }

	[MaxLength(8)]
	public string? CompanyHouseNumber { get; set; }

	public FcaStatus? FcaStatus { get; set; }

	public DateOnly? FcaStatusEffectiveDate { get; set; }

	public int? FcaReferenceNumber { get; set; }

	public string? IcoRegistrationNumber { get; set; }

	public DateOnly? IcoExpirationDate { get; set; }

	public string? VatNumber { get; set; }

	public string? VatStatus { get; set; }

	[ForeignKey(nameof(RegisteredAddress))]
	public int? RegisteredAddressId { get; set; }

	public Address? RegisteredAddress { get; set; }

	[ForeignKey(nameof(TradingAddress))]
	public int? TradingAddressId { get; set; }

	public Address? TradingAddress { get; set; }

	public IList<DealerOfficer> Officers
	{
		get => field ?? throw new UninitializedNavigationPropertyException(GetType());
		set;
	}

	public IList<DealerTradingName> TradingNames
	{
		get => field ?? throw new UninitializedNavigationPropertyException(GetType());
		set;
	}
}
