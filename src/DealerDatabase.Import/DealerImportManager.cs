using System.Collections;

using DealerDatabase.Data.Entities;

namespace DealerDatabase.Import;

public class DealerImportManager : IReadOnlyCollection<DealerEntry>
{
	private readonly Dictionary<string, DealerEntry> _lookup = [];

	public int Count => _lookup.Count;

	public IEnumerator<DealerEntry> GetEnumerator() => _lookup.Values.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public DealerEntry GetOrCreate(string name)
	{
		ArgumentException.ThrowIfNullOrEmpty(name);

		if (!_lookup.TryGetValue(name, out var data))
			_lookup[name] = data = new(name);

		return data;
	}
}

public sealed record DealerImportAddress
{
	public string? Country { get; set; }

	public string? County { get; set; }

	public string? PostalCode { get; set; }

	public string? Town { get; set; }

	public string? Line1 { get; set; }

	public string? Line2 { get; set; } = "";

	public string? Line3 { get; set; } = "";
}

public sealed record DealerImportOfficer
{
	public required string Name { get; set; }

	public required OfficerRole OfficerRole { get; set; }

	public required DateOnly AppointedOn { get; set; }

	public DateOnly? ResignedOn { get; set; }

	public string? Nationality { get; set; }

	public string? Occupation { get; set; }
}