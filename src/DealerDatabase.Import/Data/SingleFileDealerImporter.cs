using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;

using CsvHelper;

using DealerDatabase.Data;

using Microsoft.Extensions.Logging;

namespace DealerDatabase.Import.Data;

public abstract class SingleFileDealerImporter(ILogger logger, string fileName) : IDealerImporter
{
	public abstract int Priority { get; }

	protected string ImporterId => field ??= GetType().Name;

	public async Task ProcessAsync(DealerImportManager importManager, CancellationToken cancellationToken)
	{
		var file = Path.Combine(SolutionPaths.DataDirectory, fileName);
		if (!File.Exists(file))
		{
			logger.LogWarning("File for importer {Importer} not found \"{File}\"", GetType(), file);
			return;
		}

		await using var stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
		await ProcessFileAsync(stream, importManager, cancellationToken);
	}

	protected abstract Task ProcessFileAsync(FileStream stream, DealerImportManager importManager, CancellationToken cancellationToken);

	protected async Task<T> DeserializeJsonAsync<T>(Stream stream, JsonSerializerOptions options, CancellationToken cancellationToken = default)
	{
		var data = await JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken);
		if (data == null)
			throw new InvalidDataException("JSON file was null");

		return data;
	}

	protected async IAsyncEnumerable<T> DeserializeCsvAsync<T>(Stream stream, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		using var reader = new StreamReader(stream);
		using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

		await foreach(var item in csv.GetRecordsAsync<T>(cancellationToken))
			yield return item;
	}
}