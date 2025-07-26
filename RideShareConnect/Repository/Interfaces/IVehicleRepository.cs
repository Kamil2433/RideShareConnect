using RideShareConnect.Models;
using System.Threading.Tasks;

namespace RideShareConnect.Repository.Interfaces
{
    public interface IVehicleRepository
    {
        Task RegisterVehicle(Vehicle vehicle);
        Task UpdateVehicle(int vehicleId, Vehicle vehicle);
        Task DeactivateVehicle(int vehicleID);
        Task<Vehicle> GetVehicleDetails(int vehicleId);
    }
}
