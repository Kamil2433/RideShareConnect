using RideShareConnect.Models;
using System.Threading.Tasks;
using RideShareConnect.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RideShareConnect.Data;
    

namespace RideShareConnect.Repository.Implements
{
    public class VehicleService : IVehicleRepository
    {
        private readonly AppDbContext _context;
        public VehicleService(AppDbContext context) => _context = context;

        public async Task RegisterVehicle(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVehicle(int id, Vehicle vehicle)
        {
            var existing = await _context.Vehicles.FindAsync(id);
            if (existing != null)
            {
                existing.Make = vehicle.Make;
                existing.Model = vehicle.Model;
                existing.Year = vehicle.Year;
                existing.Color = vehicle.Color;
                existing.LicensePlate = vehicle.LicensePlate;
                existing.SeatingCapacity = vehicle.SeatingCapacity;
                existing.VehicleType = vehicle.VehicleType;
                existing.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeactivateVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                vehicle.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Vehicle> GetVehicleDetails(int id)
        {
            return await _context.Vehicles.FindAsync(id);
        }
    }
}
