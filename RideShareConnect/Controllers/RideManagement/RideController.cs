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


        [HttpPost("search")]
        [AllowAnonymous] // Anyone can search; change if needed
        public async Task<IActionResult> SearchRides([FromBody] RideSearchDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rides = await _rideService.SearchRidesAsync(dto);

            if (!rides.Any())
                return NotFound(new { message = "No rides found matching the criteria." });

            return Ok(rides);
        }





        [HttpPost]
        public async Task<IActionResult> PostRide([FromBody] RideCreateDto dto)
        {
            // 1. Validate request model
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 2. Get user ID from custom claim named "UserId"
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null)
                return Unauthorized(new { message = "UserId claim not found in token" });

            // 3. Parse the user ID and use it as DriverId
            int driverId = int.Parse(userIdClaim.Value);

            // 4. Call service with extracted driverId
            var result = await _rideService.CreateRideAsync(dto, driverId);

            if (result)
                return Ok(new { message = "Ride created successfully" });

            return BadRequest(new { message = "Failed to create ride" });
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
