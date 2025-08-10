using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideShareConnect.Models.DTOs;
using RideShareConnect.Services;


using System.Security.Claims;
using System.Threading.Tasks;

namespace RideShareConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Driver")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] VehicleRegistrationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Attach DriverId from authenticated user
            var userId = int.Parse(User.FindFirstValue("UserId") ?? "0");
            if (userId <= 0)
                return Unauthorized("Invalid or missing UserId claim.");

            var userIdFromDriver = await _vehicleService.GetDriverIdByUserIdAsync(userId);
            if (userIdFromDriver <= 0)
                return NotFound("Driver profile not found for the authenticated user.");


            dto.DriverId = userIdFromDriver;

            var vehicle = await _vehicleService.RegisterVehicleAsync(dto);
            return Ok(vehicle);
        }

        [HttpGet("my-vehicles")]
        public async Task<IActionResult> GetMyVehicles()
        {
            var userId = int.Parse(User.FindFirstValue("UserId") ?? "0");
            if (userId <= 0)
                return Unauthorized("Invalid or missing UserId claim.");
            var driverId = await _vehicleService.GetDriverIdByUserIdAsync(userId);
            if (driverId <= 0)
                return NotFound("Driver profile not found for the authenticated user.");


            var vehicles = await _vehicleService.GetVehiclesByDriverIdAsync(driverId);

            if (vehicles == null || vehicles.Count == 0)
                return NotFound("No vehicles found for this driver.");

            return Ok(vehicles);
        }
    }
}