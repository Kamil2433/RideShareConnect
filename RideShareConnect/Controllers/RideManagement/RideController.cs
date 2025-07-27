using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideShareConnect.Services;
using RideShareConnect.Dtos;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RideShareConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Only authenticated users can access
    public class RideController : ControllerBase
    {
        private readonly IRideService _rideService;

        public RideController(IRideService rideService)
        {
            _rideService = rideService;
        }
        // POST: api/ride/book
        [HttpPost("book")]
        public async Task<IActionResult> BookRide([FromBody] RideBookingCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get passenger id from token
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
                return Unauthorized(new { message = "UserId claim not found in token." });

            if (!int.TryParse(userIdClaim.Value, out int passengerId))
                return BadRequest(new { message = "Invalid UserId format." });

            var success = await _rideService.BookRideAsync(dto, passengerId);

            if (!success)
                return StatusCode(500, new { message = "Booking failed. Please try again." });

            return Ok(new { message = "Ride booked successfully!" });
        }


        // GET: api/ride/user
        [HttpGet("user")]
        public async Task<IActionResult> GetUserRides()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
                return Unauthorized(new { message = "UserId claim not found in token." });

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return BadRequest(new { message = "Invalid UserId format." });

            var rides = await _rideService.GetRidesByUserIdAsync(userId);

            return Ok(rides);
        }
    }
}
