using RideShareConnect.Models;
using RideShareConnect.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RideShareConnect.Repository.Interfaces
{
    public interface IVehicleRepository
    {
        Task<DriverProfileGetDto?> GetDriverProfileByUserIdAsync(int userId);
        Task<int> CreateVehicleAsync(Vehicle vehicle);
        Task<bool> UpdateVehicleAsync(Vehicle vehicle);
        Task<Vehicle> GetVehicleByIdAsync(int vehicleId);
        Task<List<Vehicle>> GetVehiclesByDriverIdAsync(int driverId);
    }
}