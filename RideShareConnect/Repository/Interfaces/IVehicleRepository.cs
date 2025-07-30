using RideShareConnect.Models;
using System.Threading.Tasks;

namespace RideShareConnect.Repository.Interfaces
{
    public interface IVehicleRepository
    {
        Task RegisterVehicle(Vehicle vehicle);
        Task UpdateVehicle(int id, Vehicle vehicle);
        Task DeactivateVehicle(int id);
        Task<Vehicle> GetVehicleDetails(int id);
    }
}
