using DealerDatabase.Data;
using DealerDatabase.Data.Entities;
using DealerDatabase.Import;
using DealerDatabase.Import.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
var assembly = typeof(Program).Assembly;

builder.Services.AddDealerDatabase();
builder.Services.AddDealerImportersFromAssembly(assembly);

using var host = builder.Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();

await using (var scope = host.Services.CreateAsyncScope())
{
	var db = scope.ServiceProvider.GetRequiredService<DealerDbContext>();
	await db.Database.MigrateAsync();
}

logger.LogInformation("Database: {DatabaseFile}", SolutionPaths.DatabaseFile);
logger.LogInformation("Data folder: {DataDirectory}", SolutionPaths.DataDirectory);

foreach (var entry in Directory.EnumerateFileSystemEntries(SolutionPaths.DataDirectory).Order())
{
	logger.LogInformation("  Found source: {Name}", Path.GetFileName(entry));
}

using var cancellationSignal = new CancellationTokenSource();

Console.CancelKeyPress += (_, _) =>
{
	if (!cancellationSignal.IsCancellationRequested)
		cancellationSignal.Cancel();
};

try
{
	var context = host.Services.GetRequiredService<DealerDbContext>();
	var importers = host.Services.GetServices<IDealerImporter>().OrderByDescending(v => v.Priority);
	var importDetails = new ImportDetails
	{
		RanAt = DateTime.UtcNow,
	};

	context.ImportDetails.Add(importDetails);

	await context.SaveChangesAsync(cancellationSignal.Token);

	var data = await RunImportersAsync(importers, cancellationSignal.Token);

	await ConsilidateDealersAsync(context, data, cancellationSignal.Token);

	importDetails.CompletedAt = DateTime.UtcNow;

	await context.SaveChangesAsync(cancellationSignal.Token);
}
catch (OperationCanceledException)
{
	logger.LogInformation("Import Cancelled.");
}

return;

async Task<DealerImportManager> RunImportersAsync(IEnumerable<IDealerImporter> importers, CancellationToken cancellationToken)
{
	var manager = new DealerImportManager();

	foreach (var importer in importers)
	{
		logger.LogInformation("Running importer {Importer}", importer.GetType());
		try
		{
			await importer.ProcessAsync(manager, cancellationToken);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Importer {Importer} failed", importer.GetType());
		}
	}

	return manager;
}

async Task ConsilidateDealersAsync(DealerDbContext context, DealerImportManager importManager, CancellationToken cancellationToken)
{
	var now = DateTime.Now;

	foreach (var dealer in importManager)
	{
		var entity = await context.Dealers
			.Include(v => v.TradingAddress)
			.Include(v => v.RegisteredAddress)
			.Include(v => v.Officers)
			.Include(v => v.TradingNames)
			.SingleOrDefaultAsync(v => dealer.NormalizedName == v.NormalizedName, cancellationToken);

		if (entity == null)
		{
			entity = new Dealer
			{
				NormalizedName = dealer.NormalizedName!,
				CreatedAt = now,
				Officers = [],
				TradingNames = [],
			};

			context.Add(entity);
		}

		var currentOfficers = entity.Officers.ToDictionary(v => v.Name);
		var currentTradingNames = entity.TradingNames.ToDictionary(v => v.Name);

		var officers = new HashSet<string>();
		var tradingNames = new HashSet<string>();

		entity.Officers.Clear();
		entity.TradingNames.Clear();

		foreach (var fields in dealer.Fields.OrderByDescending(v => v.Priority))
		{
			DealerMerger.MergeDealerFields(entity, fields);

			entity.TradingAddress = DealerMerger.MergeAddress(entity.TradingAddress, fields.TradingAddress);
			entity.RegisteredAddress = DealerMerger.MergeAddress(entity.RegisteredAddress, fields.RegisteredAddress);

			foreach (var officer in fields.Officers)
			{
				// ignore duplicates
				if (!officers.Add(officer.Name))
					continue;

				// re-use the existing row in the database if one already exists
				if (!currentOfficers.Remove(officer.Name, out var officerEntity))
				{
					officerEntity = new()
					{
						Name = officer.Name,
						OfficerRole = officer.OfficerRole,
						AppointedOn = officer.AppointedOn,
						ResignedOn = officer.ResignedOn,
						Nationality = officer.Nationality,
					};
				}
				else
				{
					officerEntity.OfficerRole = officer.OfficerRole;
					officerEntity.AppointedOn = officer.AppointedOn;
					officerEntity.ResignedOn = officer.ResignedOn;
					officerEntity.Nationality = officer.Nationality;
				}

				entity.Officers.Add(officerEntity);
			}

			foreach (var tradingName in fields.TradingNames)
			{
				// ignore duplicates
				if (!tradingNames.Add(tradingName))
					continue;

				// re-use the existing row in the database if one already exists
				if (!currentTradingNames.Remove(tradingName, out var tradingEntity))
				{
					tradingEntity = new()
					{
						Name = tradingName,
					};
				}

				entity.TradingNames.Add(tradingEntity);
			}
		}

		// prune old officer and trading name entities from the database
		context.DealerOfficers.RemoveRange(currentOfficers.Values);
		context.DealerTradingNames.RemoveRange(currentTradingNames.Values);
	}

	await context.SaveChangesAsync(cancellationToken);
}
