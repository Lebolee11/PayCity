// payCityUtilitiesApp.Api/Data/ApplicationDbContext.cs
//using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;
using PayCityApp.Api.Api.Models;
using PayCityApp.Api.Models;
using PayCityAppApi.Models;

namespace payCityUtilitiesApp.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<Meter> Meters { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Meter>()
                .HasIndex(m => m.MetreId)
                .IsUnique();

            modelBuilder.Entity<Fine>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<Meter>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId);

            modelBuilder.Entity<Transaction>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.UserId);
        }
    }
}
