using RideShareConnect.Models;
using RideShareConnect.Data;
using RideShareConnect.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace RideShareConnect.Repository.Implements
{
    public class DriverProfileService : IDriverProfileServiceRepository
    {
        private readonly AppDbContext _context;
        public DriverProfileService(AppDbContext context) => _context = context;

        public async Task CreateDriverProfile(DriverProfile profile)
        {
            await _context.DriverProfiles.AddAsync(profile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDriverProfile(int id, DriverProfile profile)
        {
            var existing = await _context.DriverProfiles.FindAsync(id);
            if (existing != null)
            {
                existing.LicenseNumber = profile.LicenseNumber;
                existing.LicenseExpiryDate = profile.LicenseExpiryDate;
                existing.LicenseImageUrl = profile.LicenseImageUrl;
                existing.YearsOfExperience = profile.YearsOfExperience;
                existing.EmergencyContactName = profile.EmergencyContactName;
                existing.EmergencyContactPhone = profile.EmergencyContactPhone;
                await _context.SaveChangesAsync();
            }
        }

        public async Task VerifyDriver(int id)
        {
            var profile = await _context.DriverProfiles.FindAsync(id);
            if (profile != null)
            {
                profile.IsVerified = true;
                profile.VerifiedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CheckLicenseExpiry(int id)
        {
            var profile = await _context.DriverProfiles.FindAsync(id);
            return profile != null && profile.LicenseExpiryDate <= DateTime.UtcNow;
        }
    }
}
