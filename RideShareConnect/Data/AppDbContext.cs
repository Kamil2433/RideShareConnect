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

            // --- User ---
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

            // --- UserProfile ---
            modelBuilder.Entity<UserProfile>()
                .HasKey(up => up.ProfileId);
            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.User)
                .WithOne()
                .HasForeignKey<UserProfile>(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- UserSettings ---
            modelBuilder.Entity<UserSettings>()
                .HasKey(us => us.SettingsId);
            modelBuilder.Entity<UserSettings>()
                .HasOne(us => us.User)
                .WithOne()
                .HasForeignKey<UserSettings>(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- TwoFactorCode ---
            modelBuilder.Entity<TwoFactorCode>()
                .HasKey(t => t.CodeId);
            modelBuilder.Entity<TwoFactorCode>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

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


               //wallet
               // Wallet - One to Many: Wallet -> WalletTransactions
            modelBuilder.Entity<Wallet>()
                .HasMany(w => w.Transactions)
                .WithOne(t => t.Wallet)
                .HasForeignKey(t => t.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional: Fluent API configuration
            modelBuilder.Entity<Wallet>()
                .Property(w => w.Balance)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<WalletTransaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");
 

            // --- Admin (optional config if needed) ---
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

            // --- Commission & Complaints ---
            // Add configuration if needed, otherwise EF will map by convention

        }
    }
}
