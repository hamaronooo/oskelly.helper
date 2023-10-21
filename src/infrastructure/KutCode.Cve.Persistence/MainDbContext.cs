using EntityFramework.Exceptions.PostgreSQL;
using KutCode.Cve.Domain.Entities;
using KutCode.Cve.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace KutCode.Cve.Persistence;

public class MainDbContext : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseExceptionProcessor();
	}

	public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }
	
	public DbSet<CveEntity> Cve { get; set; }
	public DbSet<VulnerabilityPointEntity> VulnerabilityPoints { get; set; }
	public DbSet<CveSolutionEntity> CveSolutions { get; set; }
	

	protected override void OnModelCreating(ModelBuilder mb)
	{
		mb.Entity<CveEntity>().HasKey(x => new { x.Year, x.CnaNumber });
			

		mb.Entity<CveEntity>().HasMany(x => x.Vulnerabilities)
			.WithOne(x => x.Cve)
			.HasForeignKey(x => new {x.CveYear,x.CveCnaNumber});

		mb.Entity<VulnerabilityPointEntity>().HasOne(x => x.CveSolution)
			.WithOne(x => x.VulnerabilityPoint)
			.HasForeignKey<CveSolutionEntity>(x => x.VulnerabilityPointId);

		mb.Entity<SoftwareEntity>().HasMany(x => x.VulnerabilityPoints)
			.WithOne(x => x.Software)
			.HasForeignKey(x => x.SoftwareId);

		mb.Entity<PlatformEntity>().HasMany(x => x.VulnerabilityPoints)
			.WithOne(x => x.Platform)
			.HasForeignKey(x => x.PlatformId);
	}

}