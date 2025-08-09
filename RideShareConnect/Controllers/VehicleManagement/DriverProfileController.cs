using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideShareConnect.Dtos;
using RideShareConnect.Services.Interfaces;
using System.Security.Claims;

namespace RideShareConnect.Controllers
{
    [ApiController]
    [Route("api/driver-profile")]
    [Authorize]
    public class DriverProfileController : ControllerBase
    {
        private readonly IDriverProfileService _driverProfileService;

        public DriverProfileController(IDriverProfileService driverProfileService)
        {
            _driverProfileService = driverProfileService;
        }

        [HttpGet("getdrverprofile")]
        public async Task<IActionResult> GetMyDriverProfile()
        {
            var userId = int.Parse(User.FindFirstValue("UserId") ?? "0");
            var profile = await _driverProfileService.GetDriverProfileAsync(userId);

            if (profile == null) return NotFound("Profile not found");

            return Ok(profile);
        }

        [HttpPost("create-or-update")]
        public async Task<IActionResult> CreateOrUpdateDriverProfile([FromBody] DriverProfileDto dto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId") ?? "0");
            var success = await _driverProfileService.CreateOrUpdateDriverProfileAsync(userId, dto);

            if (!success) return StatusCode(500, "Failed to save profile");

            return Ok("Profile saved successfully");
        }
    }
}