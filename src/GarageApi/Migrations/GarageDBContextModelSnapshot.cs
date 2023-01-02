﻿// <auto-generated />
using System;
using GarageDataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GarageAPI.Migrations
{
    [DbContext(typeof(GarageDBContext))]
    partial class GarageDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GarageDataBase.Tables.RecordStateTable", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("RecordStates");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "Approved"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "Waiting"
                        },
                        new
                        {
                            Id = 3L,
                            Name = "Rejected"
                        });
                });

            modelBuilder.Entity("GarageDataBase.Tables.RecordTable", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTimeOffset>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("FinishDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("LastUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PlaceNumber")
                        .HasColumnType("integer");

                    b.Property<long>("StateId")
                        .HasColumnType("bigint");

                    b.Property<string>("Time")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StateId");

                    b.HasIndex("UserId");

                    b.ToTable("Records");
                });

            modelBuilder.Entity("GarageDataBase.Tables.UserStateTable", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("UserStates");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "Clear"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "Banned"
                        });
                });

            modelBuilder.Entity("GarageDataBase.Tables.UserTable", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTimeOffset>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.Property<DateTimeOffset?>("FinishDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTimeOffset>("LastUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<long>("StateId")
                        .HasColumnType("bigint");

                    b.Property<long>("VisitCount")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("StateId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreationDate = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            Email = "ar-seny@mail.ru",
                            FirstName = "Арсений",
                            LastName = "Васильев",
                            LastUpdate = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            Patronymic = "Тестовый",
                            StateId = 1L,
                            VisitCount = 0L
                        });
                });

            modelBuilder.Entity("GarageDataBase.Tables.RecordTable", b =>
                {
                    b.HasOne("GarageDataBase.Tables.RecordStateTable", "State")
                        .WithMany("Records")
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GarageDataBase.Tables.UserTable", "User")
                        .WithMany("Records")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("State");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GarageDataBase.Tables.UserTable", b =>
                {
                    b.HasOne("GarageDataBase.Tables.UserStateTable", "State")
                        .WithMany("Users")
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("State");
                });

            modelBuilder.Entity("GarageDataBase.Tables.RecordStateTable", b =>
                {
                    b.Navigation("Records");
                });

            modelBuilder.Entity("GarageDataBase.Tables.UserStateTable", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("GarageDataBase.Tables.UserTable", b =>
                {
                    b.Navigation("Records");
                });
#pragma warning restore 612, 618
        }
    }
}
