using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Models;

namespace VisitorBook.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<VisitedState> VisitedStates { get; set; }
        public DbSet<VisitorAddress> VisitorAddresses { get; set; }

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
                .WithOne(e => e.Visitor)
                .HasForeignKey<VisitorAddress>()
                .OnDelete(DeleteBehavior.Cascade);

            // many to many relationship
            // visitor - state - visited state relationship definition
            modelBuilder.Entity<Visitor>()
               .HasMany(e => e.States)
               .WithMany(e => e.Visitors)
               .UsingEntity<VisitedState>();

            // one to one relationship
            // visitor address - city relationship definition
            modelBuilder.Entity<VisitorAddress>()
               .HasOne(e => e.City)
               .WithOne()
               .HasForeignKey<VisitorAddress>(e => e.CityId)
               .OnDelete(DeleteBehavior.NoAction)
               .IsRequired();

            // one to one relationship
            // visitor address - state relationship definition
            modelBuilder.Entity<VisitorAddress>()
               .HasOne(e => e.State)
               .WithOne()
               .HasForeignKey<VisitorAddress>(e => e.StateId)
               .OnDelete(DeleteBehavior.NoAction)
               .IsRequired();
        }
    }
}
