﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VisitorBook.DAL.Data;

#nullable disable

namespace VisitorBook.DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VisitorBook.Core.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "06",
                            Name = "Ankara"
                        },
                        new
                        {
                            Id = 2,
                            Code = "35",
                            Name = "İzmir"
                        },
                        new
                        {
                            Id = 3,
                            Code = "34",
                            Name = "İstanbul"
                        });
                });

            modelBuilder.Entity("VisitorBook.Core.Models.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("States");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CityId = 1,
                            Latitude = 39.796688099999997,
                            Longitude = 32.223354700000002,
                            Name = "Çankaya"
                        },
                        new
                        {
                            Id = 2,
                            CityId = 1,
                            Latitude = 39.905137199999999,
                            Longitude = 32.692093999999997,
                            Name = "Mamak"
                        },
                        new
                        {
                            Id = 3,
                            CityId = 1,
                            Latitude = 40.086525000000002,
                            Longitude = 32.820312000000001,
                            Name = "Keçiören"
                        },
                        new
                        {
                            Id = 4,
                            CityId = 2,
                            Latitude = 38.422052700000002,
                            Longitude = 26.964354,
                            Name = "Konak"
                        },
                        new
                        {
                            Id = 5,
                            CityId = 2,
                            Latitude = 38.478544100000001,
                            Longitude = 27.075009600000001,
                            Name = "Bayraklı"
                        },
                        new
                        {
                            Id = 6,
                            CityId = 2,
                            Latitude = 38.5013997,
                            Longitude = 26.96218,
                            Name = "Karşıyaka"
                        },
                        new
                        {
                            Id = 7,
                            CityId = 3,
                            Latitude = 40.9812333,
                            Longitude = 28.980652599999999,
                            Name = "Kadıköy"
                        },
                        new
                        {
                            Id = 8,
                            CityId = 3,
                            Latitude = 40.984420299999996,
                            Longitude = 28.974454399999999,
                            Name = "Ataşehir"
                        },
                        new
                        {
                            Id = 9,
                            CityId = 3,
                            Latitude = 41.024865200000001,
                            Longitude = 28.637796699999999,
                            Name = "Avcılar"
                        });
                });

            modelBuilder.Entity("VisitorBook.Core.Models.VisitedState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("StateId")
                        .HasColumnType("int");

                    b.Property<int>("VisitorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StateId");

                    b.HasIndex("VisitorId");

                    b.ToTable("VisitedStates");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Date = new DateTime(2015, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StateId = 2,
                            VisitorId = 1
                        },
                        new
                        {
                            Id = 2,
                            Date = new DateTime(2015, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StateId = 5,
                            VisitorId = 1
                        },
                        new
                        {
                            Id = 3,
                            Date = new DateTime(2017, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StateId = 7,
                            VisitorId = 1
                        },
                        new
                        {
                            Id = 4,
                            Date = new DateTime(2022, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StateId = 8,
                            VisitorId = 1
                        },
                        new
                        {
                            Id = 5,
                            Date = new DateTime(2012, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StateId = 4,
                            VisitorId = 2
                        },
                        new
                        {
                            Id = 6,
                            Date = new DateTime(2023, 8, 6, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StateId = 1,
                            VisitorId = 2
                        },
                        new
                        {
                            Id = 7,
                            Date = new DateTime(2010, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StateId = 1,
                            VisitorId = 3
                        },
                        new
                        {
                            Id = 8,
                            Date = new DateTime(2002, 10, 23, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StateId = 9,
                            VisitorId = 3
                        },
                        new
                        {
                            Id = 9,
                            Date = new DateTime(2011, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StateId = 9,
                            VisitorId = 3
                        },
                        new
                        {
                            Id = 10,
                            Date = new DateTime(2020, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StateId = 8,
                            VisitorId = 3
                        },
                        new
                        {
                            Id = 11,
                            Date = new DateTime(2008, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StateId = 7,
                            VisitorId = 4
                        },
                        new
                        {
                            Id = 12,
                            Date = new DateTime(2000, 8, 6, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StateId = 7,
                            VisitorId = 4
                        });
                });

            modelBuilder.Entity("VisitorBook.Core.Models.Visitor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("VisitorAddressId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VisitorAddressId")
                        .IsUnique()
                        .HasFilter("[VisitorAddressId] IS NOT NULL");

                    b.ToTable("Visitors");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BirthDate = new DateTime(1992, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = 0,
                            Name = "Eren",
                            Surname = "Gaygusuz",
                            VisitorAddressId = 1
                        },
                        new
                        {
                            Id = 2,
                            BirthDate = new DateTime(1995, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = 0,
                            Name = "Eren",
                            Surname = "Özcan"
                        },
                        new
                        {
                            Id = 3,
                            BirthDate = new DateTime(1996, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = 1,
                            Name = "Ceyda",
                            Surname = "Meyda",
                            VisitorAddressId = 2
                        },
                        new
                        {
                            Id = 4,
                            BirthDate = new DateTime(1990, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = 1,
                            Name = "Tuğçe",
                            Surname = "Güzel"
                        });
                });

            modelBuilder.Entity("VisitorBook.Core.Models.VisitorAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<int>("StateId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("VisitorAddress");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CityId = 1,
                            StateId = 1
                        },
                        new
                        {
                            Id = 2,
                            CityId = 3,
                            StateId = 7
                        });
                });

            modelBuilder.Entity("VisitorBook.Core.Models.State", b =>
                {
                    b.HasOne("VisitorBook.Core.Models.City", "City")
                        .WithMany("States")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("VisitorBook.Core.Models.VisitedState", b =>
                {
                    b.HasOne("VisitorBook.Core.Models.State", "State")
                        .WithMany()
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VisitorBook.Core.Models.Visitor", "Visitor")
                        .WithMany()
                        .HasForeignKey("VisitorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("State");

                    b.Navigation("Visitor");
                });

            modelBuilder.Entity("VisitorBook.Core.Models.Visitor", b =>
                {
                    b.HasOne("VisitorBook.Core.Models.VisitorAddress", "VisitorAddress")
                        .WithOne()
                        .HasForeignKey("VisitorBook.Core.Models.Visitor", "VisitorAddressId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("VisitorAddress");
                });

            modelBuilder.Entity("VisitorBook.Core.Models.City", b =>
                {
                    b.Navigation("States");
                });
#pragma warning restore 612, 618
        }
    }
}
