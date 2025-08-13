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
        private readonly ILogger<DriverProfileController> _logger;


        public DriverProfileController(IDriverProfileService driverProfileService, ILogger<DriverProfileController> logger)
        {
            _logger = logger;

            _driverProfileService = driverProfileService;
        }

        [HttpGet("getdrverprofile")]
        public async Task<IActionResult> GetMyDriverProfile()
        {
            var userId = int.Parse(User.FindFirstValue("UserId") ?? "0");
            var profile = await _driverProfileService.GetDriverProfileAsync(userId);
            _logger.LogDebug("Retrieved profile: {@Profile}", profile);
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