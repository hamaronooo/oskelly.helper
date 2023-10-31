﻿using EntityFramework.Exceptions.PostgreSQL;
using KutCode.Cve.Domain.Entities.Report;
using Microsoft.EntityFrameworkCore;

namespace KutCode.Cve.Application.Database;

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
	public DbSet<SoftwareEntity> Software { get; set; }
	public DbSet<PlatformEntity> Platforms { get; set; }
	public DbSet<CveResolveQueueEntity> CveResolveQueue { get; set; }

	public DbSet<ReportRequestEntity> ReportRequests { get; set; }
	public DbSet<ReportRequestVulnerabilityPointEntity> ReportRequestCve { get; set; }

	protected override void OnModelCreating(ModelBuilder mb)
	{
		mb.Entity<CveEntity>().HasKey(x => new { x.Year, x.CnaNumber });
		mb.Entity<VulnerabilityPointEntity>().HasIndex(x => x.DataSourceCode);

		mb.Entity<CveEntity>().HasMany(x => x.Vulnerabilities)
			.WithOne(x => x.Cve)
			.HasForeignKey(x => new {x.CveYear,x.CveCnaNumber});

		mb.Entity<VulnerabilityPointEntity>().HasMany(x => x.CveSolutions)
			.WithOne(x => x.VulnerabilityPoint)
			.HasForeignKey(x => x.VulnerabilityPointId);

		mb.Entity<SoftwareEntity>().HasMany(x => x.VulnerabilityPoints)
			.WithOne(x => x.Software)
			.HasForeignKey(x => x.SoftwareId);

		mb.Entity<PlatformEntity>().HasMany(x => x.VulnerabilityPoints)
			.WithOne(x => x.Platform)
			.HasForeignKey(x => x.PlatformId);
		
		//queue
		mb.Entity<CveResolveQueueEntity>().HasKey(x => new { x.CveYear, x.CveCnaNumber, x.ResolverCode });

		// report
		mb.Entity<ReportRequestEntity>().HasMany(x => x.Vulnerabilities)
			.WithOne(x => x.ReportRequest)
			.HasForeignKey(x => x.ReportRequestId);
	}

}