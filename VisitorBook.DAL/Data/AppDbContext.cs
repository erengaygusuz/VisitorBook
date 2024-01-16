using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Utilities;
using VisitorBook.Core.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace VisitorBook.DAL.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<County> Counties { get; set; }
        public DbSet<VisitedCounty> VisitedCounties { get; set; }
        public DbSet<UserAddress> UserAddress { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<SubRegion> SubRegions { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<RegisterApplication> RegisterApplications { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedingDatabase(modelBuilder);
        }

        private void SeedingDatabase(ModelBuilder modelBuilder)
        {
            #region User Management Seeding

            modelBuilder.Entity<Role>().HasData(
                new Role()
                {
                    Id = 1,
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN"
                },
                new Role()
                {
                    Id = 2,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new Role()
                {
                    Id = 3,
                    Name = "VisitorRecorder",
                    NormalizedName = "VISITORRECORDER"
                },
                new Role()
                {
                    Id = 4,
                    Name = "Visitor",
                    NormalizedName = "VISITOR"
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    Name = "Eren",
                    Surname = "Özcan",
                    BirthDate = new DateTime(year: 1990, month: 11, day: 2, hour: 11, minute: 34, second: 55),
                    Gender = Gender.Male,
                    Email = "ozcaneren@gmail.com",
                    NormalizedEmail = "OZCANEREN@GMAIL.COM",
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "12345"),
                    UserName = "erenozcan",
                    NormalizedUserName = "ERENOZCAN",
                    EmailConfirmed = true,
                    PhoneNumber = "(555) 555-5555",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                },
                new User()
                {
                    Id = 2,
                    Name = "Ceyda",
                    Surname = "Kamış",
                    BirthDate = new DateTime(year: 1996, month: 8, day: 22, hour: 1, minute: 32, second: 50),
                    Gender = Gender.Female,
                    Email = "ceydakamis@gmail.com",
                    NormalizedEmail = "CEYDAKAMIS@GMAIL.COM",
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "12345"),
                    UserName = "ceydakamis",
                    NormalizedUserName = "CEYDAKAMIS",
                    EmailConfirmed = true,
                    PhoneNumber = "(555) 555-5555",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                },
                new User()
                {
                    Id = 3,
                    Name = "Ali",
                    Surname = "Veli",
                    BirthDate = new DateTime(year: 1985, month: 4, day: 20, hour: 1, minute: 33, second: 10),
                    Gender = Gender.Male,
                    Email = "aliveli@gmail.com",
                    NormalizedEmail = "ALIVELI@GMAIL.COM",
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "12345"),
                    UserName = "aliveli",
                    NormalizedUserName = "ALIVELI",
                    EmailConfirmed = true,
                    PhoneNumber = "(555) 555-5555",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                },
                new User()
                {
                    Id = 4,
                    Name = "Serenay",
                    Surname = "Sek",
                    BirthDate = new DateTime(year: 1988, month: 11, day: 20, hour: 11, minute: 37, second: 20),
                    Gender = Gender.Female,
                    Email = "sekserenay@gmail.com",
                    NormalizedEmail = "SEKSERENAY@GMAIL.COM",
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "12345"),
                    UserName = "sekserenay",
                    NormalizedUserName = "SEKSERENAY",
                    EmailConfirmed = true,
                    PhoneNumber = "(555) 555-5555",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                }
            );

            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>
                {
                    RoleId = 1,
                    UserId = 1
                },
                new IdentityUserRole<int>
                {
                    RoleId = 2,
                    UserId = 2
                },
                new IdentityUserRole<int>
                {
                    RoleId = 3,
                    UserId = 3
                },
                new IdentityUserRole<int>
                {
                    RoleId = 4,
                    UserId = 4
                }
            );

            modelBuilder.Entity<IdentityRoleClaim<int>>().HasData(
                new IdentityRoleClaim<int>
                {
                    Id = 1,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.UserManagement.View"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 2,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.UserManagement.Create"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 3,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.UserManagement.Edit"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 4,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.UserManagement.Delete"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 5,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.PlaceManagement.View"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 6,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.PlaceManagement.Create"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 7,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.PlaceManagement.Edit"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 8,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.PlaceManagement.Delete"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 9,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.VisitedCountyManagement.View"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 10,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.VisitedCountyManagement.Create"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 11,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.VisitedCountyManagement.Edit"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 12,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.VisitedCountyManagement.Delete"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 13,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.FakeDataManagement.View"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 14,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.FakeDataManagement.Create"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 15,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.FakeDataManagement.Edit"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 16,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.FakeDataManagement.Delete"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 17,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.ContactMessageManagement.View"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 18,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.ContactMessageManagement.Create"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 19,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.ContactMessageManagement.Edit"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 20,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.ContactMessageManagement.Delete"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 21,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.AuditTrailManagement.View"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 22,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.AuditTrailManagement.Create"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 23,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.AuditTrailManagement.Edit"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 24,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.AuditTrailManagement.Delete"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 25,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.RegisterApplicationManagement.View"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 26,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.RegisterApplicationManagement.Create"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 27,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.RegisterApplicationManagement.Edit"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 28,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.RegisterApplicationManagement.Delete"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 29,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.ExceptionLogManagement.View"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 30,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.ExceptionLogManagement.Create"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 31,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.ExceptionLogManagement.Edit"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 32,
                    RoleId = 1,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.ExceptionLogManagement.Delete"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 33,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.PlaceManagement.View"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 34,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.PlaceManagement.Create"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 35,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.PlaceManagement.Edit"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 36,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.PlaceManagement.Delete"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 37,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.VisitedCountyManagement.View"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 38,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.VisitedCountyManagement.Create"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 39,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.VisitedCountyManagement.Edit"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 40,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.VisitedCountyManagement.Delete"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 41,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.ContactMessageManagement.View"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 42,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.ContactMessageManagement.Create"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 43,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.ContactMessageManagement.Edit"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 44,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.ContactMessageManagement.Delete"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 45,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.AuditTrailManagement.View"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 46,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.AuditTrailManagement.Create"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 47,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.AuditTrailManagement.Edit"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 48,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.AuditTrailManagement.Delete"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 49,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.RegisterApplicationManagement.View"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 50,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.RegisterApplicationManagement.Create"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 51,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.RegisterApplicationManagement.Edit"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 52,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.RegisterApplicationManagement.Delete"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 53,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.ExceptionLogManagement.View"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 54,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.ExceptionLogManagement.Create"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 55,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.ExceptionLogManagement.Edit"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 56,
                    RoleId = 2,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.ExceptionLogManagement.Delete"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 57,
                    RoleId = 3,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.VisitedCountyManagement.View"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 58,
                    RoleId = 3,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.VisitedCountyManagement.Create"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 59,
                    RoleId = 3,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.VisitedCountyManagement.Edit"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 60,
                    RoleId = 3,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.VisitedCountyManagement.Delete"
                },
                new IdentityRoleClaim<int>
                 {
                     Id = 61,
                     RoleId = 4,
                     ClaimType = "Permission",
                     ClaimValue = "Permissions.VisitedCountyManagement.View"
                 },
                new IdentityRoleClaim<int>
                {
                    Id = 62,
                    RoleId = 4,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.VisitedCountyManagement.Create"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 63,
                    RoleId = 4,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.VisitedCountyManagement.Edit"
                },
                new IdentityRoleClaim<int>
                {
                    Id = 64,
                    RoleId = 4,
                    ClaimType = "Permission",
                    ClaimValue = "Permissions.VisitedCountyManagement.Delete"
                }
            );

            #endregion

            #region Place Management Seeding

            modelBuilder.Entity<Region>().HasData(
                new Region()
                {
                    Id = 1,
                    Name = "Asia",
                    CreatedDate = DateTime.Now
                }
            );

            modelBuilder.Entity<SubRegion>().HasData(
                new SubRegion()
                {
                    Id = 1,
                    RegionId = 1,
                    Name = "Western Asia",
                    CreatedDate = DateTime.Now
                }
            );

            modelBuilder.Entity<Country>().HasData(
                new Country()
                {
                    Id = 1,
                    SubRegionId = 1,
                    Name = "Turkey",
                    Code = "TUR",
                    CreatedDate = DateTime.Now
                }
            );

            modelBuilder.Entity<City>().HasData(
                new City()
                {
                    Id = 1,
                    CountryId = 1,
                    Name = "Ankara",
                    Code = "06",
                    CreatedDate = DateTime.Now
                },
                new City()
                {
                    Id = 2,
                    CountryId = 1,
                    Name = "İzmir",
                    Code = "35",
                    CreatedDate = DateTime.Now
                },
                new City()
                {
                    Id = 3,
                    CountryId = 1,
                    Name = "İstanbul",
                    Code = "34",
                    CreatedDate = DateTime.Now
                }
            );

            modelBuilder.Entity<County>().HasData(
                new County()
                {
                    Id = 1,
                    Name = "Çankaya",
                    Latitude = 39.7966881,
                    Longitude = 32.2233547,
                    CityId = 1,
                    CreatedDate = DateTime.Now
                },
                new County()
                {
                    Id = 2,
                    Name = "Mamak",
                    Latitude = 39.9051372,
                    Longitude = 32.692094,
                    CityId = 1,
                    CreatedDate = DateTime.Now
                },
                new County()
                {
                    Id = 3,
                    Name = "Keçiören",
                    Latitude = 40.086525,
                    Longitude = 32.820312,
                    CityId = 1,
                    CreatedDate = DateTime.Now
                },
                new County()
                {
                    Id = 4,
                    Name = "Konak",
                    Latitude = 38.4220527,
                    Longitude = 26.964354,
                    CityId = 2,
                    CreatedDate = DateTime.Now
                },
                new County()
                {
                    Id = 5,
                    Name = "Bayraklı",
                    Latitude = 38.4785441,
                    Longitude = 27.0750096,
                    CityId = 2,
                    CreatedDate = DateTime.Now
                },
                new County()
                {
                    Id = 6,
                    Name = "Karşıyaka",
                    Latitude = 38.5013997,
                    Longitude = 26.96218,
                    CityId = 2,
                    CreatedDate = DateTime.Now
                },
                new County()
                {
                    Id = 7,
                    Name = "Kadıköy",
                    Latitude = 40.9812333,
                    Longitude = 28.9806526,
                    CityId = 3,
                    CreatedDate = DateTime.Now
                },
                new County()
                {
                    Id = 8,
                    Name = "Ataşehir",
                    Latitude = 40.9844203,
                    Longitude = 28.9744544,
                    CityId = 3,
                    CreatedDate = DateTime.Now
                },
                new County()
                {
                    Id = 9,
                    Name = "Avcılar",
                    Latitude = 41.0248652,
                    Longitude = 28.6377967,
                    CityId = 3,
                    CreatedDate = DateTime.Now
                }
            );

            #endregion
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateChangeTracker();

            OnBeforeSaveChanges();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();

            var auditEntries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditTrail || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                var auditEntry = new AuditEntry(entry);

                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.Username = GetUserName();

                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;

                    if (property.Metadata.IsPrimaryKey())
                    {
                        int propValue = (int)property.CurrentValue;

                        if (propertyName == "Id" && propValue < 0)
                        {
                            property.CurrentValue = 0;
                        }

                        auditEntry.KeyValues[propertyName] = property.CurrentValue;

                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:

                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;

                            break;

                        case EntityState.Deleted:

                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;

                            break;

                        case EntityState.Modified:

                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }

                            break;
                    }
                }
            }

            foreach (var auditEntry in auditEntries)
            {
                AuditTrails.Add(auditEntry.ToAudit());
            }
        }

        private string GetUserName()
        {
            var username = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);

            if (username == null)
            {
                return "ANONYMOUS";
            }

            return username;
        }

        public void UpdateChangeTracker()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
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
