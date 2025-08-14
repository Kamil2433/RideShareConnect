using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RideShareConnect.Dtos;
using RideShareConnect.Models;
using RideShareConnect.Repository.Interfaces;


namespace RideShareConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResetPasswordController : ControllerBase
    {
        private readonly IResetPasswordRepository _repo;

        public ResetPasswordController(IResetPasswordRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendResetPasswordOtpDto dto)
        {
            var result = await _repo.SendResetPasswordOtpAsync(dto);
            if (!result) return BadRequest("Failed to send OTP.");
            return Ok("OTP sent successfully.");
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyResetPasswordOtpDto dto)
        {
            var result = await _repo.VerifyOtpAsync(dto);
            if (!result) return BadRequest("Invalid or expired OTP.");
            return Ok("OTP verified successfully.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _repo.ResetPasswordAsync(dto);
            if (!result) return BadRequest("Failed to reset password.");
            return Ok("Password reset successfully.");
        }
    }
}
