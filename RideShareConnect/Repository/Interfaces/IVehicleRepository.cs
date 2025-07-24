using RideShareConnect.Models;
using System.Threading.Tasks;

namespace RideShareConnect.Repository.Interfaces
{
    public interface IVehicleRepository
    {
        Task RegisterVehicle(VehicleModel vehicleModel);
        Task UpdateVehicle(int vehicleId, VehicleModel vehicleModel);
        Task DeactivateVehicle(int vehicleID);
        Task<VehicleMode> GetVehicleDetails(int vehicleId);
    }
}
