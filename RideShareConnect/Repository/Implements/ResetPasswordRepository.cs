using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RideShareConnect.Data;
using RideShareConnect.Dtos;
using RideShareConnect.Models;
using RideShareConnect.Repository.Interfaces;
using RideShareConnect.Services;
using System.Security.Cryptography;
using System.Text;

namespace RideShareConnect.Repository.Implements
{
    public class ResetPasswordRepository : IResetPasswordRepository
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public ResetPasswordRepository(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<bool> SendResetPasswordOtpAsync(SendResetPasswordOtpDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return false; // Email not registered

            var otpCode = new Random().Next(100000, 999999).ToString();

            var otpEntity = new ResetPasswordOtp
            {
                Email = dto.Email,
                Otp = otpCode,
                CreatedAt = DateTime.UtcNow,
                ExpiryTime = DateTime.UtcNow.AddMinutes(5)
            };

            _context.ResetPasswordOtps.Add(otpEntity);
            await _context.SaveChangesAsync();

            Console.WriteLine($"[DEBUG] OTP for {dto.Email}: {otpCode}");

            // Send OTP via email
            try
            {
                var subject = "Your OTP for Password Reset";
                var body = $"<h3>Password Reset OTP</h3><p>Your one-time password (OTP) is: <strong>{otpCode}</strong></p><p>This OTP is valid for 5 minutes.</p>";
                await _emailService.SendEmailAsync(dto.Email, subject, body);
                Console.WriteLine($"[DEBUG] Email sent to {dto.Email} with OTP: {otpCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to send email to {dto.Email}: {ex.Message}");
                return false; // Return false if email sending fails
            }

            return true;
        }

        public async Task<bool> VerifyOtpAsync(VerifyResetPasswordOtpDto dto)
        {
            var latestOtp = await _context.ResetPasswordOtps
                .Where(o => o.Email == dto.Email)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            if (latestOtp == null)
                return false;

            if (DateTime.UtcNow > latestOtp.ExpiryTime)
                return false;

            return latestOtp.Otp == dto.OtpCode;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                return false;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return false;

            // Match AuthController hashing logic
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(dto.Password);
            var hash = sha256.ComputeHash(bytes);
            string hashedPassword = Convert.ToBase64String(hash);

            user.PasswordHash = hashedPassword;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}