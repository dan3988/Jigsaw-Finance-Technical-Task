using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DealerDatabase.Data;

public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Registers <see cref="DealerDbContext"/> using the shared SQLite database file.
	/// </summary>
	public static IServiceCollection AddDealerDatabase(this IServiceCollection services)
	{
		services.AddDbContext<DealerDbContext>(options =>
		{
			options.ConfigureWarnings(warnings => warnings.Log((RelationalEventId.CommandExecuted, LogLevel.Debug)));
			options.UseSqlite($"Data Source={SolutionPaths.DatabaseFile}");
		});

		return services;
	}
}
