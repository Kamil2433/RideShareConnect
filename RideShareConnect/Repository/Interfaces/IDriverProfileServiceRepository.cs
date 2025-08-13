using RideShareConnect.Models;

namespace RideShareConnect.Repository.Interfaces
{
    public interface IDriverProfileRepository
    {
        Task<DriverProfile?> GetByUserIdAsync(int userId);
        Task<bool> CreateAsync(DriverProfile profile);
        Task<bool> UpdateAsync(DriverProfile profile);
    }
}