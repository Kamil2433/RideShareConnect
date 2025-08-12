using RideShareConnect.Models;
using RideShareConnect.Models.DTOs;
using RideShareConnect.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RideShareConnect.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }
// Retrieves the driver ID based on the user ID
        public async Task<int> GetDriverIdByUserIdAsync(int userId)
        {
            var driverProfile = await _vehicleRepository.GetDriverProfileByUserIdAsync(userId);
            return driverProfile?.DriverProfileId ?? 0;
        }

        public async Task<VehicleDto> RegisterVehicleAsync(VehicleRegistrationDto dto)
        {
            // Check if driver already has a vehicle
            var existingVehicles = await _vehicleRepository.GetVehiclesByDriverIdAsync(dto.DriverId);
            var existingVehicle = existingVehicles.FirstOrDefault();

            if (existingVehicle != null)
            {
                // Update existing vehicle fields
                existingVehicle.VehicleType = dto.VehicleType;
                existingVehicle.LicensePlate = dto.LicensePlate;
                existingVehicle.InsuranceNumber = dto.InsuranceNumber;
                existingVehicle.RegistrationExpiry = dto.RegistrationExpiry;
                existingVehicle.RCDocumentBase64 = dto.RCDocumentBase64;
                existingVehicle.InsuranceDocumentBase64 = dto.InsuranceDocumentBase64;

                await _vehicleRepository.UpdateVehicleAsync(existingVehicle);

                // Return updated vehicle as DTO
                return MapToDto(existingVehicle);
            }
            else
            {
                // Create new vehicle
                var vehicle = new Vehicle
                {
                    DriverId = dto.DriverId,
                    VehicleType = dto.VehicleType,
                    LicensePlate = dto.LicensePlate,
                    InsuranceNumber = dto.InsuranceNumber,
                    RegistrationExpiry = dto.RegistrationExpiry,
                    RCDocumentBase64 = dto.RCDocumentBase64,
                    InsuranceDocumentBase64 = dto.InsuranceDocumentBase64
                };

                var vehicleId = await _vehicleRepository.CreateVehicleAsync(vehicle);
                var newVehicle = await _vehicleRepository.GetVehicleByIdAsync(vehicleId);
                return MapToDto(newVehicle);
            }
        }

        public async Task<bool> UpdateVehicleAsync(VehicleRegistrationDto dto)
        {
            var existingVehicle = await _vehicleRepository.GetVehicleByIdAsync(dto.VehicleId);

            if (existingVehicle == null)
                return false;

            existingVehicle.VehicleType = dto.VehicleType;
            existingVehicle.LicensePlate = dto.LicensePlate;
            existingVehicle.InsuranceNumber = dto.InsuranceNumber;
            existingVehicle.RegistrationExpiry = dto.RegistrationExpiry;
            existingVehicle.RCDocumentBase64 = dto.RCDocumentBase64;
            existingVehicle.InsuranceDocumentBase64 = dto.InsuranceDocumentBase64;

            return await _vehicleRepository.UpdateVehicleAsync(existingVehicle);
        }

        public async Task<List<VehicleDto>> GetVehiclesByDriverIdAsync(int driverId)
        {
            var vehicles = await _vehicleRepository.GetVehiclesByDriverIdAsync(driverId);
            return vehicles.Select(MapToDto).ToList();
        }

        public async Task<VehicleDto> GetVehicleByIdAsync(int vehicleId)
        {
            var vehicle = await _vehicleRepository.GetVehicleByIdAsync(vehicleId);
            return vehicle == null ? null : MapToDto(vehicle);
        }

        private VehicleDto MapToDto(Vehicle vehicle)
        {
            return new VehicleDto
            {
                VehicleId = vehicle.VehicleId,
                DriverId = vehicle.DriverId,
                VehicleType = vehicle.VehicleType,
                LicensePlate = vehicle.LicensePlate,
                InsuranceNumber = vehicle.InsuranceNumber,
                RegistrationExpiry = vehicle.RegistrationExpiry,
                RCDocumentBase64 = vehicle.RCDocumentBase64,
                InsuranceDocumentBase64 = vehicle.InsuranceDocumentBase64,
                IsApproved = vehicle.IsApproved,
                IsActive = vehicle.IsActive,
                CreatedAt = vehicle.CreatedAt,
                UpdatedAt = vehicle.UpdatedAt
            };
        }
    }
}