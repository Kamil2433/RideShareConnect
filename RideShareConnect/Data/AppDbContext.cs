using Microsoft.EntityFrameworkCore;
using RideShareConnect.Models;
using RideShareConnect.Data;

using RideShareConnect.Models.PayModel;
using RideShareConnect.Models.Admin;



namespace RideShareConnect.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Existing tables
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<TwoFactorCode> TwoFactorCodes { get; set; }
        public DbSet<ResetPasswordOtp> ResetPasswordOtps { get; set; }


        // RideShare - Vehicle & Driver Management
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<DriverProfile> DriverProfiles { get; set; }
        public DbSet<DriverRating> DriverRatings { get; set; }
        // New entities
        public DbSet<Ride> Rides { get; set; }
        public DbSet<RideBooking> RideBookings { get; set; }
        public DbSet<BookingHistory> BookingHistories { get; set; }
        public DbSet<RoutePoint> RoutePoints { get; set; }
        public DbSet<RideRequest> RideRequests { get; set; }




        public DbSet<Wallet> Wallets{get;set;}
        public DbSet<WalletTransaction> WalletTransaction { get; set; }




        // ✅ Admin module tables
         public DbSet<Admin> Admins { get; set; }
        public DbSet<Analytics> Analytics { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<Complaints> Complaints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            // --- User ---
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);
            modelBuilder.Entity<User>()
                .Property(u => u.Email).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>()
                .Property(u => u.Role).IsRequired().HasMaxLength(50);

            // UserProfile
            // --- UserProfile ---
            modelBuilder.Entity<UserProfile>()
                .HasKey(up => up.ProfileId);
            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.User)
                .WithOne()
                .HasForeignKey<UserProfile>(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserSettings
            // --- UserSettings ---
            modelBuilder.Entity<UserSettings>()
                .HasKey(us => us.SettingsId);
            modelBuilder.Entity<UserSettings>()
                .HasOne(us => us.User)
                .WithOne()
                .HasForeignKey<UserSettings>(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // TwoFactorCode
            // --- TwoFactorCode ---
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
                // --- ResetPasswordOtp ---
            modelBuilder.Entity<ResetPasswordOtp>()
                .HasKey(r => r.Id);
            modelBuilder.Entity<ResetPasswordOtp>()
                .Property(r => r.Email)
                .IsRequired()
                .HasMaxLength(255);
            modelBuilder.Entity<ResetPasswordOtp>()
                .Property(r => r.Otp)
                .IsRequired()
                .HasMaxLength(10);
            modelBuilder.Entity<ResetPasswordOtp>()
                .Property(r => r.CreatedAt)
                .IsRequired();

            // --- DriverProfile ---
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

            // --- Vehicle --- (simplified version)
            modelBuilder.Entity<Vehicle>()
                .HasKey(v => v.VehicleId);
            modelBuilder.Entity<Vehicle>()
                .Property(v => v.VehicleType).IsRequired().HasMaxLength(30);
            modelBuilder.Entity<Vehicle>()
                .Property(v => v.LicensePlate).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Vehicle>()
                .Property(v => v.InsuranceNumber).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Vehicle>()
                .Property(v => v.RCDocumentBase64).IsRequired();
            modelBuilder.Entity<Vehicle>()
                .Property(v => v.InsuranceDocumentBase64).IsRequired();

            // --- DriverRating ---
            modelBuilder.Entity<DriverRating>()
                .HasKey(r => r.RatingId);

            // --- Wallet ---
            modelBuilder.Entity<Wallet>()
                .HasMany(w => w.Transactions)
                .WithOne(t => t.Wallet)
                .HasForeignKey(t => t.WalletId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Wallet>()
                .Property(w => w.Balance).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<WalletTransaction>()
                .Property(t => t.Amount).HasColumnType("decimal(18,2)");

            // --- Admin ---
            modelBuilder.Entity<Admin>()
                .Property(a => a.Username).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Admin>()
                .Property(a => a.Email).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Admin>()
                .Property(a => a.PasswordHash).IsRequired();

            // --- Analytics ---
            modelBuilder.Entity<Analytics>()
                .Property(a => a.Date).IsRequired();
            modelBuilder.Entity<Analytics>()
                .Property(a => a.TotalRevenue).HasColumnType("decimal(18,2)");
        }
    }
}