using Microsoft.EntityFrameworkCore;
using RideShareConnect.Data;
using RideShareConnect.Models;
using RideShareConnect.Repository.Interfaces;

namespace RideShareConnect.Repository.Implements
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly AppDbContext _context;

        public UserProfileRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateProfileAsync(UserProfile profile)
        {
            await _context.UserProfiles.AddAsync(profile);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<UserProfile?> GetProfileByUserIdAsync(int userId)
        {
            return await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<bool> UpdateProfileAsync(int userId, UserProfile updatedProfile)
        {
            var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return false;

            profile.FirstName = updatedProfile.FirstName;
            profile.LastName = updatedProfile.LastName;
            profile.PhoneNumber = updatedProfile.PhoneNumber;
            profile.Address = updatedProfile.Address;
            profile.ProfilePicture = updatedProfile.ProfilePicture;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}