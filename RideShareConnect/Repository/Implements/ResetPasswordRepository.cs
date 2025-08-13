using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RideShareConnect.Data;
using RideShareConnect.Dtos;
using RideShareConnect.Models;
using RideShareConnect.Repository.Interfaces;
using BCrypt.Net;

namespace RideShareConnect.Repository.Implements
{
    public class ResetPasswordRepository : IResetPasswordRepository
    {
        private readonly AppDbContext _context;

        public ResetPasswordRepository(AppDbContext context)
        {
            _context = context;
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
                Otp = otpCode, // ✅ using Otp instead of OtpCode
                CreatedAt = DateTime.UtcNow,
                ExpiryTime = DateTime.UtcNow.AddMinutes(5)
            };

            _context.ResetPasswordOtps.Add(otpEntity);
            await _context.SaveChangesAsync();

            Console.WriteLine($"[DEBUG] OTP for {dto.Email}: {otpCode}");

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

            return latestOtp.Otp == dto.OtpCode; // ✅ comparing with Otp
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                return false;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return false;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            user.PasswordHash = hashedPassword;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
