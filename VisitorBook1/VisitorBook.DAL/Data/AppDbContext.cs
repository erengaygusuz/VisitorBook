using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Utilities;

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

        private readonly FakeDataGenerator _fakeDataGenerator;

        public AppDbContext(DbContextOptions<AppDbContext> options, FakeDataGenerator fakeDataGenerator) : base (options)
        {
            _fakeDataGenerator = fakeDataGenerator;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //_fakeDataGenerator.GenerateData();

            //modelBuilder.Entity<Visitor>().HasData(
            //    _fakeDataGenerator.Visitors
            //);

            //modelBuilder.Entity<VisitedCounty>().HasData(
            //    _fakeDataGenerator.VisitedCounties
            //);

            //modelBuilder.Entity<VisitorAddress>().HasData(
            //    _fakeDataGenerator.VisitorAddresses
            //);
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
