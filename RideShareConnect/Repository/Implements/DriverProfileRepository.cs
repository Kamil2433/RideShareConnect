using Microsoft.EntityFrameworkCore;
using RideShareConnect.Data;
using RideShareConnect.Models;
using RideShareConnect.Repository.Interfaces;

namespace RideShareConnect.Repository.Implements
{
    public class DriverProfileRepository : IDriverProfileRepository
    {
        private readonly AppDbContext _context;

        public DriverProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DriverProfile?> GetByUserIdAsync(int userId)
        {
            return await _context.DriverProfiles
                .FirstOrDefaultAsync(d => d.UserId == userId);
        }

        public async Task<bool> CreateAsync(DriverProfile profile)
        {
            await _context.DriverProfiles.AddAsync(profile);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(DriverProfile profile)
        {
            _context.DriverProfiles.Update(profile);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}