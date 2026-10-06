using DealerDatabase.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace DealerDatabase.Data;

public class DealerDbContext(DbContextOptions<DealerDbContext> options) : DbContext(options)
{
	public DbSet<ImportDetails> ImportDetails => Set<ImportDetails>();

	public DbSet<Dealer> Dealers => Set<Dealer>();

	public DbSet<DealerOfficer> DealerOfficers => Set<DealerOfficer>();

	public DbSet<DealerTradingName> DealerTradingNames => Set<DealerTradingName>();

	public DbSet<Address> Addresses => Set<Address>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
	}
}
