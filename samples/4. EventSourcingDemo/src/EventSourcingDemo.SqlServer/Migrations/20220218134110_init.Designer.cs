﻿// <auto-generated />
using System;
using EventSourcingDemo.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EventSourcingDemo.SqlServer.Migrations
{
    [DbContext(typeof(EventSourcingDemoDbContext))]
    [Migration("20220218134110_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("EventSourcingDemo")
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Brickweave.EventStore.SqlServer.Entities.EventData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CommitSequence")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Json")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("StreamId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TypeName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Event", "EventSourcingDemo");
                });

            modelBuilder.Entity("Brickweave.Messaging.SqlServer.Entities.MessageData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CommitSequence")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsSending")
                        .HasColumnType("bit");

                    b.Property<string>("Json")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastSendAttempt")
                        .HasColumnType("datetime2");

                    b.Property<int>("SendAttemptCount")
                        .HasColumnType("int");

                    b.Property<string>("TypeName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("MessageOutbox", "EventSourcingDemo");
                });

            modelBuilder.Entity("EventSourcingDemo.SqlServer.Entities.BusinessAccountData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountHolderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(12,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("AccountHolderId");

                    b.ToTable("BusinessAccount", "EventSourcingDemo");
                });

            modelBuilder.Entity("EventSourcingDemo.SqlServer.Entities.CompanyData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Company", "EventSourcingDemo");
                });

            modelBuilder.Entity("EventSourcingDemo.SqlServer.Entities.PersonalAccountData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountHolderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(12,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("AccountHolderId");

                    b.ToTable("PersonalAccount", "EventSourcingDemo");
                });

            modelBuilder.Entity("EventSourcingDemo.SqlServer.Entities.PersonData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Person", "EventSourcingDemo");
                });

            modelBuilder.Entity("EventSourcingDemo.SqlServer.Entities.BusinessAccountData", b =>
                {
                    b.HasOne("EventSourcingDemo.SqlServer.Entities.CompanyData", "AcountHolder")
                        .WithMany("Accounts")
                        .HasForeignKey("AccountHolderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AcountHolder");
                });

            modelBuilder.Entity("EventSourcingDemo.SqlServer.Entities.PersonalAccountData", b =>
                {
                    b.HasOne("EventSourcingDemo.SqlServer.Entities.PersonData", "AccountHolder")
                        .WithMany("Accounts")
                        .HasForeignKey("AccountHolderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AccountHolder");
                });

            modelBuilder.Entity("EventSourcingDemo.SqlServer.Entities.CompanyData", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("EventSourcingDemo.SqlServer.Entities.PersonData", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
