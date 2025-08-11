using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideShareConnect.Services;
using RideShareConnect.Dtos;
using System.Security.Claims;

namespace RideShareConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RideController : ControllerBase
    {
        private readonly IRideService _rideService;

        private readonly IEmailService _emailService;


        public RideController(IRideService rideService,IEmailService emailService)
        {
            _rideService = rideService;
            _emailService = emailService;

        }



        [HttpPost("booking/approve")]
        public async Task<IActionResult> ApproveOrRejectBooking([FromBody] ApproveBookingDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
                return Unauthorized(new { message = "UserId claim not found in token." });

            if (!int.TryParse(userIdClaim.Value, out int driverId))
                return BadRequest(new { message = "Invalid UserId format." });

            var result = await _rideService.ApproveOrRejectBookingAsync(dto.BookingId, driverId, dto.IsApproved);

            if (!result)
                return BadRequest(new { message = "Failed to update booking status or unauthorized action." });

            // Optionally, send an email notification to the user about the booking status

            if (dto.IsApproved)
            {
                var passengerEmail = await _rideService.GetPassengerEmailByBookingIdAsync(dto.BookingId);
                if (!string.IsNullOrEmpty(passengerEmail))
                {
                    await _emailService.SendEmailAsync(passengerEmail, "Your booking Request has been accepted", "Your booking request has been accepted by the driver. Please check your ride details on the platform.");
                }
            }
            return Ok(new { message = $"Booking {(dto.IsApproved ? "approved" : "rejected")} successfully!" });
        }



        [HttpGet("bookings")]
        public async Task<IActionResult> GetDriverBookings()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
                return Unauthorized(new { message = "UserId claim not found in token." });

            if (!int.TryParse(userIdClaim.Value, out int driverId))
                return BadRequest(new { message = "Invalid UserId format." });

            var bookings = await _rideService.GetDriverRideBookingsByStatusAsync(driverId);
            return Ok(bookings);
        }


         [HttpGet("bookingspassenger")]
        public async Task<IActionResult> GetPassengerBookings()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
                return Unauthorized(new { message = "UserId claim not found in token." });

            if (!int.TryParse(userIdClaim.Value, out int PassId))
                return BadRequest(new { message = "Invalid UserId format." });

            var bookings = await _rideService.GetDriverRideBookingsByStatusAsyncPass(PassId);
            return Ok(bookings);
        }


        [HttpPost("search")]
        [AllowAnonymous]
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
                return Unauthorized(new { message = "UserId claim not found in token" });

            if (!int.TryParse(userIdClaim.Value, out int driverId))
                return BadRequest(new { message = "Invalid UserId format" });

            var result = await _rideService.CreateRideAsync(dto, driverId);

            if (result)
                return Ok(new { message = "Ride created successfully" });

            return BadRequest(new { message = "Failed to create ride" });
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookRide([FromBody] RideBookingCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
