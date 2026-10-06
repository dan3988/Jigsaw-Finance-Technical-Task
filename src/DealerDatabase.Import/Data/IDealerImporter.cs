namespace DealerDatabase.Import.Data;

public interface IDealerImporter
{
	int Priority { get; }

	Task ProcessAsync(DealerImportManager importManager, CancellationToken cancellationToken = default);
}