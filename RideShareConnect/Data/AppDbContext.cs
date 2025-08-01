using Microsoft.EntityFrameworkCore;
using RideShareConnect.Models;

namespace RideShareConnect.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Existing entities
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<TwoFactorCode> TwoFactorCodes { get; set; }

        // New entities
        public DbSet<Ride> Rides { get; set; }
        public DbSet<RideBooking> RideBookings { get; set; }
        public DbSet<BookingHistory> BookingHistories { get; set; }
        public DbSet<RoutePoint> RoutePoints { get; set; }
        public DbSet<RideRequest> RideRequests { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
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

            // UserProfile
            modelBuilder.Entity<UserProfile>()
                .HasKey(up => up.ProfileId);
            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.User)
                .WithOne()
                .HasForeignKey<UserProfile>(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserSettings
            modelBuilder.Entity<UserSettings>()
                .HasKey(us => us.SettingsId);
            modelBuilder.Entity<UserSettings>()
                .HasOne(us => us.User)
                .WithOne()
                .HasForeignKey<UserSettings>(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // TwoFactorCode
            modelBuilder.Entity<TwoFactorCode>()
                .HasKey(t => t.CodeId);
            modelBuilder.Entity<TwoFactorCode>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ride
            modelBuilder.Entity<Ride>()
                .HasKey(r => r.RideId);
            modelBuilder.Entity<Ride>()
                .HasMany(r => r.RoutePoints)
                .WithOne(rp => rp.Ride)
                .HasForeignKey(rp => rp.RideId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Ride>()
                .HasMany(r => r.RideBookings)
                .WithOne(rb => rb.Ride)
                .HasForeignKey(rb => rb.RideId)
                .OnDelete(DeleteBehavior.Cascade);

            // RideBooking
            modelBuilder.Entity<RideBooking>()
                .HasKey(rb => rb.BookingId);
            modelBuilder.Entity<RideBooking>()
                .HasMany(rb => rb.BookingHistories)
                .WithOne(bh => bh.RideBooking)
                .HasForeignKey(bh => bh.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            // BookingHistory
            modelBuilder.Entity<BookingHistory>()
                .HasKey(bh => bh.HistoryId);

            // RoutePoint
            modelBuilder.Entity<RoutePoint>()
                .HasKey(rp => rp.RoutePointId);

            // RideRequest
            modelBuilder.Entity<RideRequest>()
                .HasKey(rr => rr.RequestId);
        }
    }
}
