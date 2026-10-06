using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace DealerDatabase.Import;

internal static partial class DataNormalizer
{
	[GeneratedRegex("[() ]")]
	private static partial Regex Phone();

	[return: NotNullIfNotNull(nameof(companyHouseNumber))]
	public static string? FormatCompanyHouseNumber(string? companyHouseNumber) =>
		string.IsNullOrEmpty(companyHouseNumber) ? companyHouseNumber : companyHouseNumber.PadLeft(8, '0');

	[return: NotNullIfNotNull(nameof(name))]
	public static string? FormatCompanyName(string? name)
	{
		if (string.IsNullOrEmpty(name))
			return name;

		name = name.Trim().ToUpper();

		if (name.EndsWith(" LIMITED"))
			name = name[..^8].TrimEnd();

		if (name.EndsWith(" LTD"))
			name = name[..^4].TrimEnd();

		if (name.EndsWith(" (UK)"))
			name = name[..^5].TrimEnd();

		return name;
	}

	[return: NotNullIfNotNull(nameof(number))]
	public static string? FormatPhoneNumber(string? number)
	{
		if (string.IsNullOrEmpty(number))
			return number;

		number = Phone().Replace(number, string.Empty);

		if (number.StartsWith('0'))
		{
			var i = 0;
			while (++i < number.Length && number[i] == '0') ;

			number = "+44" + number[i..];
		}
		else if (!number.StartsWith('+'))
		{
			number = '+' + number;
		}

		if (number.StartsWith("+4444"))
			number = '+' + number[3..];

		return number;
	}

	[return: NotNullIfNotNull(nameof(website))]
	public static string? FormatWebsite(string? website)
	{
		if (string.IsNullOrEmpty(website))
			return website;

		website = website.ToLower();

			if (!website.StartsWith("http"))
				website = "https://" + website;

		website = new Uri(website).Host;

		if (website.StartsWith("www."))
			website = website[4..];

		return website;
	}
}