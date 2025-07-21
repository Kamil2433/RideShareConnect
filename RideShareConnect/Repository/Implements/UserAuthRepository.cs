using Microsoft.EntityFrameworkCore;
using RideShareConnect.Data;
using RideShareConnect.Models;
using RideShareConnect.Repository.Interfaces;
using System.Threading.Tasks;

namespace RideShareConnect.Repository.Implements
{
    public class UserAuthRepository : IUserAuthRepository
    {
        private readonly AppDbContext _context;

        public UserAuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task AddTwoFactorCodeAsync(TwoFactorCode code)
        {
            await _context.TwoFactorCodes.AddAsync(code);
        }

        public async Task<TwoFactorCode> GetValidTwoFactorCodeAsync(string email, string code)
        {
            return await _context.TwoFactorCodes
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.User.Email == email && t.Code == code && !t.IsUsed && t.ExpiresAt > DateTime.UtcNow);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}