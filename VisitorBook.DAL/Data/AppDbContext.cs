using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using VisitorBook.Core.Models;

namespace VisitorBook.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<County> Counties { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<VisitedCounty> VisitedCounties { get; set; }
        public DbSet<VisitorAddress> VisitorAddress { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Data Seeding 

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

            modelBuilder.Entity<County>().HasData(
                new County()
                {
                    Id = 1,
                    Name = "Çankaya",
                    Latitude = 39.7966881,
                    Longitude = 32.2233547,
                    CityId = 1
                },
                new County()
                {
                    Id = 2,
                    Name = "Mamak",
                    Latitude = 39.9051372,
                    Longitude = 32.692094,
                    CityId = 1
                },
                new County()
                {
                    Id = 3,
                    Name = "Keçiören",
                    Latitude = 40.086525,
                    Longitude = 32.820312,
                    CityId = 1
                },
                new County()
                {
                    Id = 4,
                    Name = "Konak",
                    Latitude = 38.4220527,
                    Longitude = 26.964354,
                    CityId = 2
                },
                new County()
                {
                    Id = 5,
                    Name = "Bayraklı",
                    Latitude = 38.4785441,
                    Longitude = 27.0750096,
                    CityId = 2
                },
                new County()
                {
                    Id = 6,
                    Name = "Karşıyaka",
                    Latitude = 38.5013997,
                    Longitude = 26.96218,
                    CityId = 2
                },
                new County()
                {
                    Id = 7,
                    Name = "Kadıköy",
                    Latitude = 40.9812333,
                    Longitude = 28.9806526,
                    CityId = 3
                },
                new County()
                {
                    Id = 8,
                    Name = "Ataşehir",
                    Latitude = 40.9844203,
                    Longitude = 28.9744544,
                    CityId = 3
                },
                new County()
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
                    CountyId = 1
                },
                new VisitorAddress()
                {
                    Id = 2,
                    CountyId = 7
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

            modelBuilder.Entity<VisitedCounty>().HasData(
                new VisitedCounty()
                {
                    Id = 1,
                    VisitorId = 1,
                    CountyId = 2,
                    VisitDate = new DateTime(day: 2, month: 11, year: 2015)
                },
                new VisitedCounty()
                {
                    Id = 2,
                    VisitorId = 1,
                    CountyId = 5,
                    VisitDate = new DateTime(day: 4, month: 10, year: 2015)
                },
                new VisitedCounty()
                {
                    Id = 3,
                    VisitorId = 1,
                    CountyId = 7,
                    VisitDate = new DateTime(day: 24, month: 1, year: 2017)
                },
                new VisitedCounty()
                {
                    Id = 4,
                    VisitorId = 1,
                    CountyId = 8,
                    VisitDate = new DateTime(day: 16, month: 8, year: 2022)
                },
                new VisitedCounty()
                {
                    Id = 5,
                    VisitorId = 2,
                    CountyId = 4,
                    VisitDate = new DateTime(day: 15, month: 12, year: 2012)
                },
                new VisitedCounty()
                {
                    Id = 6,
                    VisitorId = 2,
                    CountyId = 1,
                    VisitDate = new DateTime(day: 6, month: 8, year: 2023)
                },
                new VisitedCounty()
                {
                    Id = 7,
                    VisitorId = 3,
                    CountyId = 1,
                    VisitDate = new DateTime(day: 1, month: 7, year: 2010)
                },
                new VisitedCounty()
                {
                    Id = 8,
                    VisitorId = 3,
                    CountyId = 9,
                    VisitDate = new DateTime(day: 23, month: 10, year: 2002)
                },
                new VisitedCounty()
                {
                    Id = 9,
                    VisitorId = 3,
                    CountyId = 9,
                    VisitDate = new DateTime(day: 15, month: 2, year: 2011)
                },
                new VisitedCounty()
                {
                    Id = 10,
                    VisitorId = 3,
                    CountyId = 8,
                    VisitDate = new DateTime(day: 16, month: 5, year: 2020)
                },
                new VisitedCounty()
                {
                    Id = 11,
                    VisitorId = 4,
                    CountyId = 7,
                    VisitDate = new DateTime(day: 1, month: 12, year: 2008)
                },
                new VisitedCounty()
                {
                    Id = 12,
                    VisitorId = 4,
                    CountyId = 7,
                    VisitDate = new DateTime(day: 6, month: 8, year: 2000)
                }
            );

            #endregion
        }

        public override int SaveChanges()
        {
            UpdateChangeTracker();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateChangeTracker();

            return base.SaveChangesAsync(cancellationToken);
        }

        public void UpdateChangeTracker()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseModel entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                Entry(entityReference).Property(x => x.UpdatedDate).IsModified = false;
                                entityReference.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;

                                entityReference.UpdatedDate = DateTime.Now;
                                break;
                            }
                    }
                }
            }
        }
    }
}
