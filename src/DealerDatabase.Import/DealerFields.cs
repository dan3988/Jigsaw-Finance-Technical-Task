using DealerDatabase.Data.Entities;

namespace DealerDatabase.Import;

public record DealerFields(string Source, int Priority)
{
	public string Source { get; } = Source;

	public int Priority { get; } = Priority;

	public string? LegalName { get; set; }

	public string? CompanyHouseNumber { get; set; }

	public string? PhoneNumber { get; set; }

	public string? Website { get; set; }

	public string? Email { get; set; }

	public DateOnly? IncorporationDate { get; set; }

	public string? MarketDealerId { get; set; }

	public FcaStatus? FcaStatus { get; set; }

	public DateOnly? FcaStatusEffectiveDate { get; set; }

	public int? FcaReferenceNumber { get; set; }

	public string? IcoRegistrationNumber { get; set; }

	public DateOnly? IcoExpirationDate { get; set; }

	public string? VatNumber { get; set; }

	public string? VatStatus { get; set; }

	public DealerImportAddress? RegisteredAddress { get; set; }

	public DealerImportAddress? TradingAddress { get; set; }

	public List<DealerImportOfficer> Officers { get; } = [];

	public List<string> TradingNames { get; } = [];
}