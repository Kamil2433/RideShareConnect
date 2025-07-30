using Microsoft.EntityFrameworkCore;
using RideShareConnect.Models;

namespace RideShareConnect.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<TwoFactorCode> TwoFactorCodes { get; set; }

        // Module 3: RideShare - Vehicle & Driver Management
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleDocument> VehicleDocuments { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
        public DbSet<DriverProfile> DriverProfiles { get; set; }
        public DbSet<DriverRating> DriverRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(50);

            // Configure UserProfile
            modelBuilder.Entity<UserProfile>()
                .HasKey(up => up.ProfileId);
            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.User)
                .WithOne()
                .HasForeignKey<UserProfile>(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure UserSettings
            modelBuilder.Entity<UserSettings>()
                .HasKey(us => us.SettingsId);
            modelBuilder.Entity<UserSettings>()
                .HasOne(us => us.User)
                .WithOne()
                .HasForeignKey<UserSettings>(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure TwoFactorCode
            modelBuilder.Entity<TwoFactorCode>()
                .HasKey(t => t.CodeId);
            modelBuilder.Entity<TwoFactorCode>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

                	
            modelBuilder.Entity<DriverProfile>()
                .HasKey(d => d.DriverProfileId);

            modelBuilder.Entity<DriverProfile>()
                .HasMany(d => d.Vehicles)
                .WithOne(v => v.Driver)
                .HasForeignKey(v => v.DriverId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DriverProfile>()
                .HasMany(d => d.DriverRatings)
                .WithOne(r => r.Driver)
                .HasForeignKey(r => r.DriverId)
                .OnDelete(DeleteBehavior.Cascade);

            // ----- VEHICLE -----
            modelBuilder.Entity<Vehicle>()
                .HasKey(v => v.VehicleId);
            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.VehicleDocuments)
                .WithOne(d => d.Vehicle)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.MaintenanceRecords)
                .WithOne(m => m.Vehicle)
                .HasForeignKey(m => m.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            // ----- VEHICLE DOCUMENT -----
            modelBuilder.Entity<VehicleDocument>()
                .HasKey(d => d.DocumentId);

            // ----- MAINTENANCE RECORD -----
            modelBuilder.Entity<MaintenanceRecord>()
                .HasKey(m => m.MaintenanceId);
                
            // ----- DRIVER RATING -----
            modelBuilder.Entity<DriverRating>()
                .HasKey(r => r.RatingId);
        }
    }
}