using DealerDatabase.Data.Entities;

namespace DealerDatabase.Import;

public static class DealerMerger
{
	public static void MergeDealerFields(Dealer entity, DealerFields fields)
	{
		entity.IncorporationDate ??= fields.IncorporationDate;
		entity.FcaStatus ??= fields.FcaStatus;
		entity.FcaStatusEffectiveDate ??= fields.FcaStatusEffectiveDate;
		entity.FcaReferenceNumber ??= fields.FcaReferenceNumber;
		entity.IcoExpirationDate ??= fields.IcoExpirationDate;

		if (string.IsNullOrEmpty(entity.CompanyHouseNumber))
			entity.CompanyHouseNumber = fields.CompanyHouseNumber;

		if (string.IsNullOrEmpty(entity.LegalName))
			entity.LegalName = fields.LegalName;

		if (string.IsNullOrEmpty(entity.IcoRegistrationNumber))
			entity.IcoRegistrationNumber = fields.IcoRegistrationNumber;

		if (string.IsNullOrEmpty(entity.PhoneNumber))
			entity.PhoneNumber = fields.PhoneNumber;

		if (string.IsNullOrEmpty(entity.Website))
			entity.Website = fields.Website;

		if (string.IsNullOrEmpty(entity.Email))
			entity.Email = fields.Email;

		if (string.IsNullOrEmpty(entity.VatNumber))
			entity.VatNumber = fields.VatNumber;

		if (string.IsNullOrEmpty(entity.VatStatus))
			entity.VatStatus = fields.VatStatus;
	}

	public static Address? MergeAddress(Address? entity, DealerImportAddress? importAddress)
	{
		if (importAddress == null)
			return entity;

		if ((entity == null || string.IsNullOrEmpty(entity.Country)) && !string.IsNullOrEmpty(importAddress.Country))
		{
			entity ??= new();
			entity.Country = importAddress.Country;
		}

		if ((entity == null || string.IsNullOrEmpty(entity.County)) && !string.IsNullOrEmpty(importAddress.County))
		{
			entity ??= new();
			entity.County = importAddress.County;
		}

		if ((entity == null || string.IsNullOrEmpty(entity.PostalCode)) && !string.IsNullOrEmpty(importAddress.PostalCode))
		{
			entity ??= new();
			entity.PostalCode = importAddress.PostalCode;
		}

		if ((entity == null || string.IsNullOrEmpty(entity.Town)) && !string.IsNullOrEmpty(importAddress.Town))
		{
			entity ??= new();
			entity.Town = importAddress.Town;
		}

		if ((entity == null || string.IsNullOrEmpty(entity.Line1)) && !string.IsNullOrEmpty(importAddress.Line1))
		{
			entity ??= new();
			entity.Line1 = importAddress.Line1;
		}

		if ((entity == null || string.IsNullOrEmpty(entity.Line2)) && !string.IsNullOrEmpty(importAddress.Line2))
		{
			entity ??= new();
			entity.Line2 = importAddress.Line2;
		}

		if ((entity == null || string.IsNullOrEmpty(entity.Line3)) && !string.IsNullOrEmpty(importAddress.Line3))
		{
			entity ??= new();
			entity.Line3 = importAddress.Line3;
		}

		return entity;
	}
}