using CsvHelper.Configuration.Attributes;

using Microsoft.Extensions.Logging;

namespace DealerDatabase.Import.Data;

public class CrawledDealerImporter(ILogger<CrawledDealerImporter> logger) : SingleFileDealerImporter(logger, "crawled_dealers.csv")
{
	public override int Priority => ImporterPriority.Crawler;

	protected override async Task ProcessFileAsync(FileStream stream, DealerImportManager importManager, CancellationToken cancellationToken)
	{
		await foreach (var row in DeserializeCsvAsync<CsvRow>(stream, cancellationToken))
		{
			if (row.BusinessNameDetected == "")
				continue;

			var normalizedName = DataNormalizer.FormatCompanyName(row.BusinessNameDetected);
			var dealer = importManager.GetOrCreate(normalizedName);
			var fields = dealer.AddFields(ImporterId, Priority);
			var phoneNumbers = row.PhonesDetected.Split([',', '|', ';']);

			fields.LegalName = row.BusinessNameDetected;
			fields.PhoneNumber = DataNormalizer.FormatPhoneNumber(phoneNumbers.FirstOrDefault());
			fields.Email = row.EmailsDetected.Split(';').FirstOrDefault();
			fields.Website = DataNormalizer.FormatWebsite(row.SourceUrl);
			fields.TradingAddress = new()
			{
				Line1 = row.AddressDetected,
				PostalCode = row.PostcodeDetected
			};
		}
	}

	private record CsvRow
	{
		[Name("crawl_id")]
		public required string CrawlId { get; set; }

		[Name("crawled_at")]
		public DateTime CrawledAt { get; set; }

		[Name("source_url")]
		public required string SourceUrl { get; set; }

		[Name("final_url")]
		public required string FinalUrl { get; set; }

		[Name("http_status")]
		public int HttpStatus { get; set; }

		[Name("page_type")]
		public required string PageType { get; set; }

		// SEO & Scraping Metadata
		[Name("page_title")]
		public required string PageTitle { get; set; }

		[Name("h1")]
		public required string H1 { get; set; }

		[Name("meta_description")]
		public required string MetaDescription { get; set; }

		// Extracted Identity & Contact Vector Information
		[Name("business_name_detected")]
		public required string BusinessNameDetected { get; set; }

		[Name("address_detected")]
		public required string AddressDetected { get; set; }

		[Name("postcode_detected")]
		public required string PostcodeDetected { get; set; }

		[Name("phones_detected")]
		public required string PhonesDetected { get; set; }

		[Name("emails_detected")]
		public required string EmailsDetected { get; set; }

		[Name("footer_text")]
		public required string FooterText { get; set; }

		// Detected Corporate & Regulatory Anchors
		[Name("company_number_detected")]
		public required string CompanyNumberDetected { get; set; }

		[Name("vat_number_detected")]
		public required string VatNumberDetected { get; set; }

		[Name("fca_frn_detected")]
		public required string FcaFrnDetected { get; set; }

		// Financial & Compliance Indicators
		[Name("finance_calculator")]
		public required string FinanceCalculator { get; set; }

		[Name("finance_lenders_detected")]
		public required string FinanceLendersDetected { get; set; }

		[Name("representative_apr")]
		public required string RepresentativeApr { get; set; }

		// Architectural & Platform Tech Stack Info
		[Name("website_platform")]
		public required string WebsitePlatform { get; set; }

		[Name("cms_version")]
		public required string CmsVersion { get; set; }

		// Public Social & Reputation Proofing Scores
		[Name("google_rating")]
		public required string GoogleRating { get; set; }

		[Name("google_review_count")]
		public required string GoogleReviewCount { get; set; }

		[Name("trustpilot_score")]
		public required string TrustpilotScore { get; set; }

		[Name("facebook_url")]
		public required string FacebookUrl { get; set; }

		[Name("instagram_url")]
		public required string InstagramUrl { get; set; }

		// Operational Context Flags
		[Name("opening_hours_raw")]
		public required string OpeningHoursRaw { get; set; }

		[Name("stock_count_detected")]
		public required string StockCountDetected { get; set; }

		[Name("part_exchange")]
		public required string PartExchange { get; set; }

		[Name("warranty_offered")]
		public required string WarrantyOffered { get; set; }

		// Performance & Payload Audit Fields
		[Name("cookie_banner")]
		public required string CookieBanner { get; set; }

		[Name("word_count")]
		public required string WordCount { get; set; }

		[Name("response_ms")]
		public required string ResponseMs { get; set; }

		[Name("content_hash")]
		public required string ContentHash { get; set; }

		[Name("lang")]
		public required string Lang { get; set; }
	}
}