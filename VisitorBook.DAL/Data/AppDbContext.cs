using Microsoft.EntityFrameworkCore;
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

        private readonly FakeDataGenerator _fakeDataGenerator;

        public AppDbContext(DbContextOptions<AppDbContext> options, FakeDataGenerator fakeDataGenerator) : base (options)
        {
            _fakeDataGenerator = fakeDataGenerator;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _fakeDataGenerator.GenerateData();

            modelBuilder.Entity<City>().HasData(
                _fakeDataGenerator.Cities
            );

            modelBuilder.Entity<County>().HasData(
                _fakeDataGenerator.Counties
            );

            modelBuilder.Entity<Visitor>().HasData(
                _fakeDataGenerator.Visitors
            );

            modelBuilder.Entity<VisitedCounty>().HasData(
                _fakeDataGenerator.VisitedCounties
            );

            modelBuilder.Entity<VisitorAddress>().HasData(
                _fakeDataGenerator.VisitorAddresses
            );
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
