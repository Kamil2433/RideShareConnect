using RideShareConnect.Dtos;

namespace RideShareConnect.Services.Interfaces
{
    public interface IDriverProfileService
    {
        Task<DriverProfileDto> GetDriverProfileAsync(int userId);
        Task<bool> CreateOrUpdateDriverProfileAsync(int userId, DriverProfileDto dto);
    }
}