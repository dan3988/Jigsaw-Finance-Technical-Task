using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

using DealerDatabase.Data.Entities;

using Microsoft.Extensions.Logging;

namespace DealerDatabase.Import.Data;

public class CompaniesHouseImporter(ILogger<CompaniesHouseImporter> logger) : SingleFileDealerImporter(logger, "companies_house.json")
{
	private static readonly JsonSerializerOptions SerializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
		UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
	};

	private static readonly ImmutableDictionary<string, OfficerRole> OfficerRoles = ImmutableDictionary.CreateRange([
		KeyValuePair.Create("director", OfficerRole.Director),
		KeyValuePair.Create("secretary", OfficerRole.Secretary)
	]);

	public override int Priority => ImporterPriority.CompaniesHouse;

	protected override async Task ProcessFileAsync(FileStream stream, DealerImportManager importManager, CancellationToken cancellationToken)
	{
		var data = await DeserializeJsonAsync<CompaniesHouseData>(stream, SerializerOptions, cancellationToken);

		logger.LogInformation("Found {count} items", data.Items.Length);

		foreach (var item in data.Items)
		{
			var normalizedName = DataNormalizer.FormatCompanyName(item.CompanyName);
			var dealer = importManager.GetOrCreate(normalizedName);
			var fields = dealer.AddFields(ImporterId, Priority);
			var address = item.RegisteredOfficeAddress;

			fields.LegalName = item.CompanyName;
			fields.CompanyHouseNumber = DataNormalizer.FormatCompanyHouseNumber(item.CompanyNumber);
			fields.IncorporationDate = item.DateOfCreation;
			fields.TradingAddress = new()
			{
				Line1 = address.AddressLine1,
				Line2 = address.AddressLine2,
				Line3 = address.AddressLine3,
				Country = address.Country,
				County = address.Region,
				Town = address.Locality,
				PostalCode = address.PostalCode,
			};

			fields.Officers.AddRange(item.Officers.Select(v => new DealerImportOfficer
			{
				Name = v.Name,
				OfficerRole = OfficerRoles[v.OfficerRole],
				AppointedOn = v.AppointedOn,
				ResignedOn = v.ResignedOn,
				Occupation = v.Occupation,
				Nationality = v.Nationality,
			}));
		}
	}

	private record CompaniesHouseData
	{
		public required string Source { get; init; }

		public required DateTime ExtractedAt { get; init; }

		public required int ItemsCount { get; init; }

		public required Company[] Items { get; init; }
	}

	private record Company
	{
		public required string CompanyNumber { get; init; }

		public required string CompanyName { get; init; }

		public required string CompanyStatus { get; init; }

		public required string Type { get; init; }

		public required DateOnly DateOfCreation { get; init; }

		public DateOnly? DateOfCessation { get; init; }

		public required CompanyAddress RegisteredOfficeAddress { get; init; }

		public required string[] SicCodes { get; init; }

		public required CompanyOfficer[] Officers { get; init; }

		public CompanyName[] PreviousCompanyNames { get; init; } = [];
	}

	private record CompanyAddress
	{
		[JsonPropertyName("address_line_1")]
		public required string AddressLine1 { get; init; }

		[JsonPropertyName("address_line_2")]
		public string? AddressLine2 { get; init; }

		[JsonPropertyName("address_line_3")]
		public string? AddressLine3 { get; init; }

		public required string Locality { get; init; }

		public string? Region { get; init; }

		public required string PostalCode { get; init; }

		public required string Country { get; init; }
	}

	private record CompanyOfficer
	{
		public required string Name { get; init; }

		public required string OfficerRole { get; init; }

		public required DateOnly AppointedOn { get; init; }

		public DateOnly? ResignedOn { get; init; }

		public string? Occupation { get; init; }

		public string? Nationality { get; init; }
	}

	private record CompanyName
	{
		public required string Name { get; init; }

		public required DateOnly EffectiveFrom { get; init; }

		public required DateOnly CeasedOn { get; init; }
	}
}