﻿// <auto-generated />
using System;
using KutCode.Cve.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KutCode.Cve.Persistence.Migrations
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

                    b.Property<double?>("CVSS")
                        .HasColumnType("double precision")
                        .HasColumnName("cvss");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.HasKey("Year", "CnaNumber");

                    b.ToTable("cve");
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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("SolutionLink")
                        .HasColumnType("text")
                        .HasColumnName("solution_link");

                    b.Property<Guid>("VulnerabilityPointId")
                        .HasColumnType("uuid")
                        .HasColumnName("vulnerability_point_id");

                    b.HasKey("Id");

                    b.HasIndex("VulnerabilityPointId")
                        .IsUnique();

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

                    b.Property<Guid?>("PlatformId")
                        .HasColumnType("uuid")
                        .HasColumnName("platform_id");

                    b.Property<string>("PlatformVersion")
                        .HasColumnType("text")
                        .HasColumnName("platform_version");

                    b.Property<Guid?>("SoftwareId")
                        .HasColumnType("uuid")
                        .HasColumnName("software_id");

                    b.Property<string>("SoftwareVersion")
                        .HasColumnType("text")
                        .HasColumnName("software_version");

                    b.HasKey("Id");

                    b.HasIndex("PlatformId");

                    b.HasIndex("SoftwareId");

                    b.HasIndex("CveYear", "CveCnaNumber");

                    b.ToTable("vulnerability_point");
                });

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.CveSolutionEntity", b =>
                {
                    b.HasOne("KutCode.Cve.Domain.Entities.VulnerabilityPointEntity", "VulnerabilityPoint")
                        .WithOne("CveSolution")
                        .HasForeignKey("KutCode.Cve.Domain.Entities.CveSolutionEntity", "VulnerabilityPointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VulnerabilityPoint");
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

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.SoftwareEntity", b =>
                {
                    b.Navigation("VulnerabilityPoints");
                });

            modelBuilder.Entity("KutCode.Cve.Domain.Entities.VulnerabilityPointEntity", b =>
                {
                    b.Navigation("CveSolution");
                });
#pragma warning restore 612, 618
        }
    }
}
