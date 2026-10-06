using CsvHelper.Configuration.Attributes;

using Microsoft.Extensions.Logging;

namespace DealerDatabase.Import.Data;

public sealed class IcoRegisterImporter(ILogger<IcoRegisterImporter> logger) : SingleFileDealerImporter(logger, "ico_register.csv")
{
	public override int Priority => ImporterPriority.Ico;

	protected override async Task ProcessFileAsync(FileStream stream, DealerImportManager importManager, CancellationToken cancellationToken)
	{
		await foreach (var record in DeserializeCsvAsync<IcoRecord>(stream, cancellationToken))
		{
			var normalizedName = DataNormalizer.FormatCompanyName(record.OrganisationName);
			var dealer = importManager.GetOrCreate(normalizedName);
			var fields = dealer.AddFields(ImporterId, Priority);

			fields.IcoRegistrationNumber = record.RegistrationNumber;
			fields.CompanyHouseNumber = DataNormalizer.FormatCompanyHouseNumber(record.CompanyRegistrationNumber);
			fields.TradingNames.AddRange(record.TradingNames.Split(';').Select(v => v.Trim()));
			fields.RegisteredAddress = new()
			{
				Line1 = record.OrganisationAddressLine1,
				Line2 = record.OrganisationAddressLine2,
				Line3 = record.OrganisationAddressLine3,
				PostalCode = record.OrganisationPostcode,
			};
		}
	}

	private record IcoRecord
	{
		[Name("Registration_number")]
		public required string RegistrationNumber { get; init; }

		[Name("Organisation_name")]
		public required string OrganisationName { get; init; }

		[Name("Company_registration_number")]
		public required string CompanyRegistrationNumber { get; init; }

		[Name("Organisation_address_line_1")]
		public required string OrganisationAddressLine1 { get; init; }

		[Name("Organisation_address_line_2")]
		public required string OrganisationAddressLine2 { get; init; }

		[Name("Organisation_address_line_3")]
		public required string OrganisationAddressLine3 { get; init; }

		[Name("Organisation_postcode")]
		public required string OrganisationPostcode { get; init; }

		[Name("Public_authority")]
		public required string PublicAuthority { get; init; }

		[Name("Start_date_of_registration")]
		public required string StartDateOfRegistration { get; init; }

		[Name("End_date_of_registration")]
		public required string EndDateOfRegistration { get; init; }

		[Name("Payment_tier")]
		public required string PaymentTier { get; init; }

		[Name("Trading_names")]
		public required string TradingNames { get; init; }
	}
}