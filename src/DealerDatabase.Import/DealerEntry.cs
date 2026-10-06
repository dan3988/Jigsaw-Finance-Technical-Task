namespace DealerDatabase.Import;

public sealed class DealerEntry
{
	private readonly List<DealerFields> _fields = [];

	public string? NormalizedName { get; }

	public IReadOnlyList<DealerFields> Fields => _fields;

	internal DealerEntry(string normalizedName)
	{
		NormalizedName = normalizedName;
	}

	public DealerFields AddFields(string source, int priority)
	{
		var field = new DealerFields(source, priority);
		_fields.Add(field);
		return field;
	}
}