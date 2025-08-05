using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideShareConnect.Dtos;
using RideShareConnect.Models;
using RideShareConnect.Repository.Interfaces;
using System.Security.Claims;

namespace RideShareConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileRepository _repo;
        private readonly IMapper _mapper;

        public UserProfileController(IUserProfileRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateMyProfile([FromBody] UserProfileDto dto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId") ?? "0");

            // Check if profile already exists
            var existingProfile = await _repo.GetProfileByUserIdAsync(userId);
            if (existingProfile != null)
            {
                return BadRequest("Profile already exists. Use PUT or POST /me to update.");
            }

            var profile = _mapper.Map<UserProfile>(dto);
            profile.UserId = userId;

            var success = await _repo.CreateProfileAsync(profile);

            if (!success)
            {
                return StatusCode(500, "Failed to create profile.");
            }

            return Ok("Profile created successfully.");
        }




        // GET /api/UserProfile/me
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = int.Parse(User.FindFirstValue("UserId") ?? "0");
            Console.WriteLine(userId);
            var profile = await _repo.GetProfileByUserIdAsync(userId);

            if (profile == null) return NotFound("Profile not found");

            var dto = _mapper.Map<UserProfileDto>(profile);
            return Ok(dto);
        }

        // POST /api/UserProfile/me
        [Authorize]
        [HttpPost("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UserProfileDto dto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId") ?? "0");

            var profile = _mapper.Map<UserProfile>(dto);
            var success = await _repo.UpdateProfileAsync(userId, profile);

            if (!success) return NotFound("Profile not found to update");

            return Ok("Profile updated");
        }
    }
}