using RideShareConnect.Models;
using System.Threading.Tasks;

namespace RideShareConnect.Repository.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<bool> CreateProfileAsync(UserProfile profile);
        Task<UserProfile?> GetProfileByUserIdAsync(int userId);
        Task<bool> UpdateProfileAsync(int userId, UserProfile updatedProfile);
    }
}
