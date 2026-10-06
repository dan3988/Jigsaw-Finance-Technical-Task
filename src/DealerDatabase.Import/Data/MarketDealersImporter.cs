using CsvHelper.Configuration.Attributes;

using Microsoft.Extensions.Logging;

namespace DealerDatabase.Import.Data;

public class MarketDealersImporter(ILogger<MarketDealersImporter> logger) : SingleFileDealerImporter(logger, "marketcheck_dealers.csv")
{
	private static CsvRow NormalizeRow(CsvRow record) => record with
	{
		SellerName = DataNormalizer.FormatCompanyName(record.SellerName)
	};

	public override int Priority => ImporterPriority.Market;

	protected override async Task ProcessFileAsync(FileStream stream, DealerImportManager importManager, CancellationToken cancellationToken)
	{
		await foreach (var row in DeserializeCsvAsync<CsvRow>(stream, cancellationToken).Select(NormalizeRow).WithCancellation(cancellationToken))
		{
			var normalizedName = DataNormalizer.FormatCompanyName(row.SellerName);
			var dealer = importManager.GetOrCreate(normalizedName);
			var fields = dealer.AddFields(ImporterId, Priority);

			fields.LegalName = row.SellerName;
			fields.MarketDealerId = row.McDealerId;
			fields.PhoneNumber = DataNormalizer.FormatPhoneNumber(row.Phone);
			fields.Website = DataNormalizer.FormatWebsite(row.Website);
			fields.TradingAddress = new()
			{
				Line1 = row.Street,
				County = row.County,
				PostalCode = row.Postcode,
				Town = row.City,
			};
		}
	}

	private record CsvRow
	{
		[Name("mc_dealer_id")]
		public required string McDealerId { get; set; }

		[Name("seller_name")]
		public required string SellerName { get; set; }

		[Name("seller_type")]
		public required string SellerType { get; set; }

		[Name("franchise_make")]
		public required string? FranchiseMake { get; set; }

		[Name("street")]
		public required string? Street { get; set; }

		[Name("city")]
		public required string? City { get; set; }

		[Name("county")]
		public required string? County { get; set; }

		[Name("postcode")]
		public required string? Postcode { get; set; }

		[Name("phone")]
		public required string? Phone { get; set; }

		[Name("website")]
		public required string? Website { get; set; }

		[Name("email")]
		public required string? Email { get; set; }

		// Stock Metrics
		[Name("inventory_count")]
		public required string InventoryCount { get; set; }

		[Name("avg_listed_price")]
		public required string AvgListedPrice { get; set; }

		[Name("avg_sold_price")]
		public required string AvgSoldPrice { get; set; }

		[Name("avg_days_in_stock")]
		public required string AvgDaysInStock { get; set; }

		[Name("sold_last_30_days")]
		public required string SoldLast30Days { get; set; }

		// Stock Distribution Types
		[Name("vehicle_types")]
		public required string VehicleTypes { get; set; }

		[Name("car_pct")]
		public required string CarPct { get; set; }

		[Name("van_pct")]
		public required string VanPct { get; set; }

		// Granular Age & Mileage Breakdowns
		[Name("cnt_under_80k_miles_under_7yrs")]
		public required string CntUnder80kMilesUnder7Yrs { get; set; }

		[Name("cnt_under_100k_miles_under_10yrs")]
		public required string CntUnder100kMilesUnder10Yrs { get; set; }

		[Name("cnt_under_120k_miles_under_12yrs")]
		public required string CntUnder120kMilesUnder12Yrs { get; set; }

		[Name("cnt_over_20yrs_over_50k_miles")]
		public required string CntOver20YrsOver50kMiles { get; set; }

		// Price Band Breakdowns
		[Name("cnt_price_over_10k")]
		public required string CntPriceOver10k { get; set; }

		[Name("cnt_price_over_20k")]
		public required string CntPriceOver20k { get; set; }

		[Name("cnt_price_over_50k")]
		public required string CntPriceOver50k { get; set; }

		// Metadata tracking
		[Name("stock_feed_provider")]
		public required string StockFeedProvider { get; set; }

		[Name("last_seen")]
		public required DateTime LastSeen { get; set; }
	}
}