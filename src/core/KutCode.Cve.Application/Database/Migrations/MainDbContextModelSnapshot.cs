﻿// <auto-generated />
using System;
using KutCode.Cve.Application.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KutCode.Cve.Application.Database.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.CveEntity", b =>
                {
                    b.Property<int>("Year")
                        .HasColumnType("integer")
                        .HasColumnName("year");

                    b.Property<string>("CnaNumber")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("cna_number");

                    b.Property<double?>("CvssMaximumRate")
                        .HasColumnType("double precision")
                        .HasColumnName("cvss_max_rate");

                    b.Property<string>("DescriptionEnglish")
                        .HasColumnType("text")
                        .HasColumnName("description_en");

                    b.Property<string>("DescriptionRussian")
                        .HasColumnType("text")
                        .HasColumnName("description_ru");

                    b.Property<bool>("Locked")
                        .HasColumnType("boolean")
                        .HasColumnName("locked");

                    b.Property<string>("ShortName")
                        .HasColumnType("text")
                        .HasColumnName("short_name");

                    b.HasKey("Year", "CnaNumber");

                    b.ToTable("cve");
                });

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.CveResolveQueueEntity", b =>
                {
                    b.Property<int>("CveYear")
                        .HasColumnType("integer")
                        .HasColumnName("cve_year");

                    b.Property<string>("CveCnaNumber")
                        .HasColumnType("text")
                        .HasColumnName("cve_cna_number");

                    b.Property<string>("ResolverCode")
                        .HasColumnType("text")
                        .HasColumnName("resolver_code");

                    b.Property<int>("Priority")
                        .HasColumnType("integer")
                        .HasColumnName("priority");

                    b.Property<DateTime>("SysCreated")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("sys_created");

                    b.Property<bool>("UpdateCve")
                        .HasColumnType("boolean")
                        .HasColumnName("update_cve");

                    b.HasKey("CveYear", "CveCnaNumber", "ResolverCode");

                    b.ToTable("cve_resolve_queue");
                });

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.CveSolutionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AdditionalLink")
                        .HasColumnType("text")
                        .HasColumnName("additional_link");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("DownloadLink")
                        .HasColumnType("text")
                        .HasColumnName("download_link");

                    b.Property<string>("Info")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("info");

                    b.Property<string>("SolutionLink")
                        .HasColumnType("text")
                        .HasColumnName("solution_link");

                    b.Property<Guid>("VulnerabilityPointId")
                        .HasColumnType("uuid")
                        .HasColumnName("vulnerability_point_id");

                    b.HasKey("Id");

                    b.HasIndex("VulnerabilityPointId");

                    b.ToTable("cve_solution");
                });

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.PlatformEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("PlatformType")
                        .HasColumnType("integer")
                        .HasColumnName("platform_type");

                    b.HasKey("Id");

                    b.ToTable("platform");
                });

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.Report.ReportRequestCveEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("CveCnaNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("cve_cna_number");

                    b.Property<int>("CveYear")
                        .HasColumnType("integer")
                        .HasColumnName("cve_year");

                    b.Property<string>("Platform")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("platform");

                    b.Property<Guid>("ReportRequestId")
                        .HasColumnType("uuid")
                        .HasColumnName("report_request_id");

                    b.Property<string>("Software")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("software");

                    b.HasKey("Id");

                    b.HasIndex("ReportRequestId");

                    b.ToTable("report_request_cve");
                });

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.Report.ReportRequestEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("CustomName")
                        .HasColumnType("text")
                        .HasColumnName("custom_name");

                    b.Property<int>("SearchStrategy")
                        .HasColumnType("integer")
                        .HasColumnName("search_strategy");

                    b.Property<string>("SourcesRaw")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("sources");

                    b.Property<int>("State")
                        .HasColumnType("integer")
                        .HasColumnName("state");

                    b.HasKey("Id");

                    b.ToTable("report_request");
                });

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.SoftwareEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("software");
                });

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.VulnerabilityPointEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("CveCnaNumber")
                        .IsRequired()
                        .HasColumnType("character varying(20)")
                        .HasColumnName("cve_cna_number");

                    b.Property<int>("CveYear")
                        .HasColumnType("integer")
                        .HasColumnName("cve_year");

                    b.Property<string>("DataSourceCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("data_source_code");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Impact")
                        .HasColumnType("text")
                        .HasColumnName("impact");

                    b.Property<Guid?>("PlatformId")
                        .HasColumnType("uuid")
                        .HasColumnName("platform_id");

                    b.Property<string>("ShortName")
                        .HasColumnType("text")
                        .HasColumnName("short_name");

                    b.Property<Guid?>("SoftwareId")
                        .HasColumnType("uuid")
                        .HasColumnName("software_id");

                    b.HasKey("Id");

                    b.HasIndex("DataSourceCode");

                    b.HasIndex("PlatformId");

                    b.HasIndex("SoftwareId");

                    b.HasIndex("CveYear", "CveCnaNumber");

                    b.ToTable("vulnerability_point");
                });

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.CveSolutionEntity", b =>
                {
                    b.HasOne("KutCode.Cve.Domain.Entities.VulnerabilityPointEntity", "VulnerabilityPoint")
                        .WithMany("CveSolutions")
                        .HasForeignKey("VulnerabilityPointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VulnerabilityPoint");
                });

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.Report.ReportRequestCveEntity", b =>
                {
                    b.HasOne("KutCode.Cve.Domain.Entities.Report.ReportRequestEntity", "ReportRequest")
                        .WithMany("Cve")
                        .HasForeignKey("ReportRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ReportRequest");
                });

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.VulnerabilityPointEntity", b =>
                {
                    b.HasOne("KutCode.Cve.Domain.Entities.PlatformEntity", "Platform")
                        .WithMany("VulnerabilityPoints")
                        .HasForeignKey("PlatformId");

                    b.HasOne("KutCode.Cve.Domain.Entities.SoftwareEntity", "Software")
                        .WithMany("VulnerabilityPoints")
                        .HasForeignKey("SoftwareId");

                    b.HasOne("KutCode.Cve.Domain.Entities.CveEntity", "Cve")
                        .WithMany("Vulnerabilities")
                        .HasForeignKey("CveYear", "CveCnaNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cve");

                    b.Navigation("Platform");

                    b.Navigation("Software");
                });

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.CveEntity", b =>
                {
                    b.Navigation("Vulnerabilities");
                });

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.PlatformEntity", b =>
                {
                    b.Navigation("VulnerabilityPoints");
                });

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.Report.ReportRequestEntity", b =>
                {
                    b.Navigation("Cve");
                });

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.SoftwareEntity", b =>
                {
                    b.Navigation("VulnerabilityPoints");
                });

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.VulnerabilityPointEntity", b =>
                {
                    b.Navigation("CveSolutions");
                });
#pragma warning restore 612, 618
        }
    }
}
