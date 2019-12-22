﻿// <auto-generated />
using System;
using Exico.HF.DbAccess.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Exico.HF.DbAccess.Migrations
{
    [DbContext(typeof(ExicoHfDbContext))]
    [Migration("20191222024839_AddedWOrkerAsebly")]
    partial class AddedWOrkerAsebly
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Exico.HF.DbAccess.Db.Models.HfUserJob", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("HfJobId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("JobType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TimeZoneId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("WorkDataId")
                        .HasColumnType("int");

                    b.Property<string>("WorkerAssemblyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkerClassName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HfUserJob");
                });

            modelBuilder.Entity("Exico.HF.DbAccess.Db.Models.HfUserRecurringJob", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CronExpression")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HfUserJobId")
                        .HasColumnType("int");

                    b.Property<string>("LastHfJobId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastRun")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("NextRun")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("HfUserJobId");

                    b.ToTable("HfUserRecurringJob");
                });

            modelBuilder.Entity("Exico.HF.DbAccess.Db.Models.HfUserScheduledJob", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("HfUserJobId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ScheduledAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("HfUserJobId");

                    b.ToTable("HfUserScheduledJob");
                });

            modelBuilder.Entity("Exico.HF.DbAccess.Db.Models.HfUserRecurringJob", b =>
                {
                    b.HasOne("Exico.HF.DbAccess.Db.Models.HfUserJob", "HfUserJob")
                        .WithMany()
                        .HasForeignKey("HfUserJobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Exico.HF.DbAccess.Db.Models.HfUserScheduledJob", b =>
                {
                    b.HasOne("Exico.HF.DbAccess.Db.Models.HfUserJob", "HfUserJob")
                        .WithMany()
                        .HasForeignKey("HfUserJobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}