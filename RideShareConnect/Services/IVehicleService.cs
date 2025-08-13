using RideShareConnect.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RideShareConnect.Services
{
    public interface IVehicleService
    {

        Task<int> GetDriverIdByUserIdAsync(int userId);
        Task<VehicleDto> RegisterVehicleAsync(VehicleRegistrationDto dto);
        // Task<bool> UpdateVehicleAsync(VehicleUpdateDto dto);
        Task<bool> UpdateVehicleAsync(VehicleRegistrationDto dto);
        Task<VehicleDto> GetVehicleByIdAsync(int vehicleId);
        Task<List<VehicleDto>> GetVehiclesByDriverIdAsync(int driverId);
    }
}