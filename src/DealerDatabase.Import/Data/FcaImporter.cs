using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

using DealerDatabase.Data.Entities;

using Microsoft.Extensions.Logging;

namespace DealerDatabase.Import.Data;

public class FcaImporter(ILogger<FcaImporter> logger) : SingleFileDealerImporter(logger, "fca_register.json")
{
	private static readonly JsonSerializerOptions SerializerOptions = new()
	{
		UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
	};

	private static readonly ImmutableDictionary<string, FcaStatus> StatusMap = ImmutableDictionary.CreateRange([
		KeyValuePair.Create("Appointed representative", FcaStatus.AppointedRepresentative),
		KeyValuePair.Create("Authorised", FcaStatus.Authorised),
		KeyValuePair.Create("No longer authorised", FcaStatus.NoLongerAuthorised)
	]);

	public override int Priority => ImporterPriority.Fca;

	protected override async Task ProcessFileAsync(FileStream stream, DealerImportManager importManager, CancellationToken cancellationToken)
	{
		var data = await DeserializeJsonAsync<ApiResponse>(stream, SerializerOptions, cancellationToken);

		foreach (var item in data.Data)
		{
			var normalizedName = DataNormalizer.FormatCompanyName(item.OrganisationName);
			var dealer = importManager.GetOrCreate(normalizedName);
			var fields = dealer.AddFields(ImporterId, Priority);

			fields.LegalName = item.OrganisationName;
			fields.CompanyHouseNumber = DataNormalizer.FormatCompanyHouseNumber(item.CompaniesHouseNumber);
			fields.FcaReferenceNumber = item.Frn.ValueKind == JsonValueKind.Number ? item.Frn.GetInt32() : int.Parse(item.Frn.GetString()!);
			fields.FcaStatus = StatusMap[item.Status];
			fields.FcaStatusEffectiveDate = DateOnly.Parse(item.StatusEffectiveDate);
			fields.TradingNames.AddRange(item.TradingNames);
			fields.RegisteredAddress = new()
			{
				Line1 = item.Address.AddressLine1,
				Line2 = item.Address.AddressLine2,
				PostalCode = item.Address.Postcode,
				Town = item.Address.Town,
			};
		}
	}

	private record ApiResponse
	{
		[JsonPropertyName("Message")]
		public required string Message { get; set; }

		[JsonPropertyName("ResultInfo")]
		public required ResultInfo ResultInfo { get; set; }

		[JsonPropertyName("Data")]
		public required OrganisationData[] Data { get; set; }
	}

	private record ResultInfo
	{
		[JsonPropertyName("total_count")]
		public required int TotalCount { get; set; }

		[JsonPropertyName("exported")]
		public required DateTime Exported { get; set; }
	}

	private record OrganisationData
	{
		// FRN alternates between String and Number in the JSON source.
		// Using JsonElement allows you to safely call .ToString() or .GetInt32() without errors.
		[JsonPropertyName("FRN")]
		public required JsonElement Frn { get; set; }

		[JsonPropertyName("Organisation Name")]
		public required string OrganisationName { get; set; }

		[JsonPropertyName("Status")]
		public required string Status { get; set; }

		[JsonPropertyName("Status Effective Date")]
		public required string StatusEffectiveDate { get; set; }

		[JsonPropertyName("Business Type")]
		public required string BusinessType { get; set; }

		[JsonPropertyName("Companies House Number")]
		public string? CompaniesHouseNumber { get; set; }

		[JsonPropertyName("Trading Names")]
		public required string[] TradingNames { get; set; }

		[JsonPropertyName("Address")]
		public required Address Address { get; set; }

		[JsonPropertyName("Principal Firm")]
		public PrincipalFirm? PrincipalFirm { get; set; }

		[JsonPropertyName("Permissions")]
		public required string[] Permissions { get; set; }
	}

	private record Address
	{
		[JsonPropertyName("Address Line 1")]
		public required string AddressLine1 { get; set; }

		[JsonPropertyName("Address Line 2")]
		public string? AddressLine2 { get; set; }

		[JsonPropertyName("Town")]
		public required string Town { get; set; }

		[JsonPropertyName("Postcode")]
		public required string Postcode { get; set; }
	}

	private record PrincipalFirm
	{
		[JsonPropertyName("FRN")]
		public required int Frn { get; set; }

		[JsonPropertyName("Name")]
		public required string Name { get; set; }
	}
}