using Microsoft.EntityFrameworkCore;
using RideShareConnect.Models;

namespace RideShareConnect.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSettings>(entity =>
            {
                entity.HasKey(u => u.SettingsId); // Explicitly set primary key (already handled by [Key])
                entity.HasOne(u => u.User)
                      .WithOne() // One-to-one relationship
                      .HasForeignKey<UserSettings>(u => u.UserId)
                      .IsRequired(); // Enforce the required foreign key
            });
        }
    }
}