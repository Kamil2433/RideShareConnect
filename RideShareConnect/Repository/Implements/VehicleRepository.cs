using Microsoft.EntityFrameworkCore;
using RideShareConnect.Data;
using RideShareConnect.Models;
using RideShareConnect.Dtos;
using RideShareConnect.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RideShareConnect.Repository.Implements
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _context;


        // Constructor to inject the database context   
        public VehicleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DriverProfileGetDto?> GetDriverProfileByUserIdAsync(int userId)
        {
            var driverProfile = await _context.DriverProfiles
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (driverProfile == null)
                return null;

            // Manual mapping
            return new DriverProfileGetDto
            {
                DriverProfileId = driverProfile.DriverProfileId,
        
           
            };
        }
// Create a new vehicle and return its ID
        public async Task<int> CreateVehicleAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
            return vehicle.VehicleId;
        }

        public async Task<bool> UpdateVehicleAsync(Vehicle vehicle)
        {
            vehicle.UpdatedAt = DateTime.UtcNow;
            _context.Vehicles.Update(vehicle);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int vehicleId)
        {
            return await _context.Vehicles.FindAsync(vehicleId);
        }

        public async Task<List<Vehicle>> GetVehiclesByDriverIdAsync(int driverId)
        {
            return await _context.Vehicles
                .Where(v => v.DriverId == driverId)
                .ToListAsync();
        }
    }
}