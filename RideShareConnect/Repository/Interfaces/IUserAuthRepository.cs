using RideShareConnect.Models;
using System.Threading.Tasks;

namespace RideShareConnect.Repository.Interfaces
{
    public interface IUserAuthRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task AddTwoFactorCodeAsync(TwoFactorCode code);
        Task<TwoFactorCode> GetValidTwoFactorCodeAsync(string email, string code);
        Task UpdateUserAsync(User user);
        Task SaveChangesAsync();
    }
}