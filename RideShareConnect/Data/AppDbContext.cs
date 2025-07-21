// File: Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using RideShareConnect.Models;

namespace RideShareConnect.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserVerification> UserVerifications { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }
        public DbSet<TwoFactorCode> TwoFactorCodes { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        // public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.ProfileId);
                entity.HasOne(e => e.User)
                      .WithOne()
                      .HasForeignKey<UserProfile>(e => e.UserId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserVerification>(entity =>
            {
                entity.HasKey(e => e.VerificationId);
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.DocumentType)
                      .HasConversion<string>()
                      .HasMaxLength(50);
                entity.Property(e => e.Status)
                      .HasConversion<string>()
                      .HasMaxLength(50);
            });

            modelBuilder.Entity<UserSettings>(entity =>
            {
                entity.HasKey(e => e.SettingsId);
                entity.HasOne(e => e.User)
                      .WithOne()
                      .HasForeignKey<UserSettings>(e => e.UserId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LoginHistory>(entity =>
            {
                entity.HasKey(e => e.LoginId);
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TwoFactorCode>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.DeliveryMethod)
                      .HasConversion<string>()
                      .HasMaxLength(20);
                entity.Property(e => e.Status)
                      .HasConversion<string>()
                      .HasMaxLength(20);
            });

            modelBuilder.Entity<EmailVerification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.Status)
                      .HasConversion<string>()
                      .HasMaxLength(20);
            });

            // modelBuilder.Entity<PasswordResetToken>(entity =>
            // {
            //     entity.HasKey(e => e.Id);
            //     entity.HasOne(e => e.User)
            //           .WithMany()
            //           .HasForeignKey(e => e.UserId)
            //           .IsRequired()
            //           .OnDelete(DeleteBehavior.Cascade);
            // });
        }
    }
}
