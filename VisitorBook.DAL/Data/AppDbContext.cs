using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using VisitorBook.Core.Models;

namespace VisitorBook.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<VisitedState> VisitedStates { get; set; }
        public DbSet<VisitorAddress> VisitorAddress { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // one to many relationship
            // city - state relationship definition
            modelBuilder.Entity<City>()
               .HasMany(e => e.States)
               .WithOne(e => e.City)
               .HasForeignKey(e => e.CityId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();

            // one to one relationship
            // visitor - visitor address relationship definition
            modelBuilder.Entity<Visitor>()
               .HasOne(e => e.VisitorAddress)
               .WithOne()
               .HasForeignKey<Visitor>(e => e.VisitorAddressId)
               .OnDelete(DeleteBehavior.Cascade);

            // many to many relationship
            // visitor - state - visited state relationship definition
            modelBuilder.Entity<Visitor>()
               .HasMany(e => e.States)
               .WithMany(e => e.Visitors)
               .UsingEntity<VisitedState>();            

            // data seeding
            modelBuilder.Entity<City>().HasData(
                new City()
                {
                    Id = 1,
                    Name = "Ankara",
                    Code = "06"
                },
                new City()
                {
                    Id = 2,
                    Name = "İzmir",
                    Code = "35"
                },
                new City()
                {
                    Id = 3,
                    Name = "İstanbul",
                    Code = "34"
                }
            );

            modelBuilder.Entity<State>().HasData(
                new State()
                {
                    Id = 1,
                    Name = "Çankaya",
                    Latitude = 39.7966881,
                    Longitude = 32.2233547,
                    CityId = 1
                },
                new State()
                {
                    Id = 2,
                    Name = "Mamak",
                    Latitude = 39.9051372,
                    Longitude = 32.692094,
                    CityId = 1
                },
                new State()
                {
                    Id = 3,
                    Name = "Keçiören",
                    Latitude = 40.086525,
                    Longitude = 32.820312,
                    CityId = 1
                },
                new State()
                {
                    Id = 4,
                    Name = "Konak",
                    Latitude = 38.4220527,
                    Longitude = 26.964354,
                    CityId = 2
                },
                new State()
                {
                    Id = 5,
                    Name = "Bayraklı",
                    Latitude = 38.4785441,
                    Longitude = 27.0750096,
                    CityId = 2
                },
                new State()
                {
                    Id = 6,
                    Name = "Karşıyaka",
                    Latitude = 38.5013997,
                    Longitude = 26.96218,
                    CityId = 2
                },
                new State()
                {
                    Id = 7,
                    Name = "Kadıköy",
                    Latitude = 40.9812333,
                    Longitude = 28.9806526,
                    CityId = 3
                },
                new State()
                {
                    Id = 8,
                    Name = "Ataşehir",
                    Latitude = 40.9844203,
                    Longitude = 28.9744544,
                    CityId = 3
                },
                new State()
                {
                    Id = 9,
                    Name = "Avcılar",
                    Latitude = 41.0248652,
                    Longitude = 28.6377967,
                    CityId = 3
                }
            );

            modelBuilder.Entity<VisitorAddress>().HasData(
                new VisitorAddress()
                {
                    Id = 1,
                    StateId = 1,
                    CityId = 1,
                },
                new VisitorAddress()
                {
                    Id = 2,
                    StateId = 7,
                    CityId = 3,
                }
            );

            modelBuilder.Entity<Visitor>().HasData(
                new Visitor()
                {
                    Id = 1,
                    Name = "Eren",
                    Surname = "Gaygusuz",
                    BirthDate = new DateTime(day: 14, month: 12, year: 1992),
                    Gender = Gender.Man,
                    VisitorAddressId = 1
                },
                new Visitor()
                {
                    Id = 2,
                    Name = "Eren",
                    Surname = "Özcan",
                    BirthDate = new DateTime(day: 05, month: 11, year: 1995),
                    Gender = Gender.Man,
                    VisitorAddressId = null
                },
                new Visitor()
                {
                    Id = 3,
                    Name = "Ceyda",
                    Surname = "Meyda",
                    BirthDate = new DateTime(day: 22, month: 3, year: 1996),
                    Gender = Gender.Woman,
                    VisitorAddressId = 2
                },
                new Visitor()
                {
                    Id = 4,
                    Name = "Tuğçe",
                    Surname = "Güzel",
                    BirthDate = new DateTime(day: 11, month: 5, year: 1990),
                    Gender = Gender.Woman,
                    VisitorAddressId = null
                }
            );

            modelBuilder.Entity<VisitedState>().HasData(
                new VisitedState()
                {
                    Id = 1,
                    VisitorId = 1,
                    StateId = 2,
                    Date = new DateTime(day: 2, month: 11, year: 2015)
                },
                new VisitedState()
                {
                    Id = 2,
                    VisitorId = 1,
                    StateId = 5,
                    Date = new DateTime(day: 4, month: 10, year: 2015)
                },
                new VisitedState()
                {
                    Id = 3,
                    VisitorId = 1,
                    StateId = 7,
                    Date = new DateTime(day: 24, month: 1, year: 2017)
                },
                new VisitedState()
                {
                    Id = 4,
                    VisitorId = 1,
                    StateId = 8,
                    Date = new DateTime(day: 16, month: 8, year: 2022)
                },
                new VisitedState()
                {
                    Id = 5,
                    VisitorId = 2,
                    StateId = 4,
                    Date = new DateTime(day: 15, month: 12, year: 2012)
                },
                new VisitedState()
                {
                    Id = 6,
                    VisitorId = 2,
                    StateId = 1,
                    Date = new DateTime(day: 6, month: 8, year: 2023)
                },
                new VisitedState()
                {
                    Id = 7,
                    VisitorId = 3,
                    StateId = 1,
                    Date = new DateTime(day: 1, month: 7, year: 2010)
                },
                new VisitedState()
                {
                    Id = 8,
                    VisitorId = 3,
                    StateId = 9,
                    Date = new DateTime(day: 23, month: 10, year: 2002)
                },
                new VisitedState()
                {
                    Id = 9,
                    VisitorId = 3,
                    StateId = 9,
                    Date = new DateTime(day: 15, month: 2, year: 2011)
                },
                new VisitedState()
                {
                    Id = 10,
                    VisitorId = 3,
                    StateId = 8,
                    Date = new DateTime(day: 16, month: 5, year: 2020)
                },
                new VisitedState()
                {
                    Id = 11,
                    VisitorId = 4,
                    StateId = 7,
                    Date = new DateTime(day: 1, month: 12, year: 2008)
                },
                new VisitedState()
                {
                    Id = 12,
                    VisitorId = 4,
                    StateId = 7,
                    Date = new DateTime(day: 6, month: 8, year: 2000)
                }
            );
        }
    }
}
