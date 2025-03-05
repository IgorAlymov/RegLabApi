﻿// <auto-generated />
using System;
using ConfigurationManager.ConfigurationManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ConfigurationManager.Migrations
{
    [DbContext(typeof(ConfigurationDbContext))]
    [Migration("20250305084605_AddedBaseConfiguration")]
    partial class AddedBaseConfiguration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ConfigurationManager.ConfigurationManager.Domain.Entities.BaseConfigurationVersion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ConfigurationId")
                        .HasColumnType("uuid");

                    b.Property<int>("ConfigurationType")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("DateAdded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("DateUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ConfigurationId");

                    b.ToTable("ConfigurationVersions");

                    b.HasDiscriminator<int>("ConfigurationType");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("ConfigurationManager.ConfigurationManager.Domain.Entities.Configuration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CurrentVersionId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("DateAdded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("DateUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Name", "UserId")
                        .IsUnique();

                    b.ToTable("Configurations");
                });

            modelBuilder.Entity("ConfigurationManager.ConfigurationManager.Domain.Entities.ColorSchemesConfigurationVersion", b =>
                {
                    b.HasBaseType("ConfigurationManager.ConfigurationManager.Domain.Entities.BaseConfigurationVersion");

                    b.Property<string>("ConfigurationData")
                        .IsRequired()
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("jsonb")
                        .HasColumnName("ConfigurationData");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("ConfigurationManager.ConfigurationManager.Domain.Entities.FontsConfigurationVersion", b =>
                {
                    b.HasBaseType("ConfigurationManager.ConfigurationManager.Domain.Entities.BaseConfigurationVersion");

                    b.Property<string>("ConfigurationData")
                        .IsRequired()
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("jsonb")
                        .HasColumnName("ConfigurationData");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("ConfigurationManager.ConfigurationManager.Domain.Entities.BaseConfigurationVersion", b =>
                {
                    b.HasOne("ConfigurationManager.ConfigurationManager.Domain.Entities.Configuration", null)
                        .WithMany("ConfigurationVersions")
                        .HasForeignKey("ConfigurationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ConfigurationManager.ConfigurationManager.Domain.Entities.Configuration", b =>
                {
                    b.Navigation("ConfigurationVersions");
                });
#pragma warning restore 612, 618
        }
    }
}
