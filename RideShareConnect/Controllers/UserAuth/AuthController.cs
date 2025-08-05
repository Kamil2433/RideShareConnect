using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RideShareConnect.Dtos;
using RideShareConnect.Models;
using RideShareConnect.Repository.Interfaces;
using RideShareConnect.Services;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace RideShareConnect.Controllers.UserAuth
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthRepository _userAuthRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthController(IUserAuthRepository userAuthRepository, IEmailService emailService, IMapper mapper, IConfiguration configuration)
        {
            _userAuthRepository = userAuthRepository;
            _emailService = emailService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserAuthRegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userAuthRepository.GetUserByEmailAsync(registerDto.Email);
            if (existingUser != null)
                return Conflict("Email already exists.");

            if (registerDto.Role != "Driver" && registerDto.Role != "Passenger" && registerDto.Role != "Admin")
                return BadRequest("Invalid role. Must be Driver, Passenger, or Admin.");

            var user = _mapper.Map<User>(registerDto);
            user.PasswordHash = HashPassword(registerDto.Password);
            await _userAuthRepository.AddUserAsync(user);
            await _userAuthRepository.SaveChangesAsync();

            var otp = GenerateOtp();
            var twoFactorCode = new TwoFactorCode
            {
                UserId = user.UserId,
                Code = otp,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                IsUsed = false
            };
            await _userAuthRepository.AddTwoFactorCodeAsync(twoFactorCode);
            await _userAuthRepository.SaveChangesAsync();

            await _emailService.SendEmailAsync(user.Email, "Verify Your Email for Rideshare", $"Your OTP for email verification is {otp}");

            return Ok(new UserAuthRegisterResponseDto { Message = "Registration successful. Please check your email for the OTP to verify your account." });
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] UserAuthVerifyEmailDto verifyEmailDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var twoFactorCode = await _userAuthRepository.GetValidTwoFactorCodeAsync(verifyEmailDto.Email, verifyEmailDto.Code);
            if (twoFactorCode == null)
                return BadRequest("Invalid or expired OTP.");

            var user = await _userAuthRepository.GetUserByEmailAsync(verifyEmailDto.Email);
            if (user == null)
                return NotFound("User not found.");

            user.IsEmailVerified = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _userAuthRepository.UpdateUserAsync(user);
            twoFactorCode.IsUsed = true;
            await _userAuthRepository.SaveChangesAsync();

            return Ok(new UserAuthRegisterResponseDto { Message = "Email verified successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserAuthLoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userAuthRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
                return Unauthorized("Invalid email or password.");

            if (!user.IsEmailVerified)
                return BadRequest("Email not verified.");

            var token = GenerateJwtToken(user);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // For local HTTP
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddMinutes(60),
                Path = "/",
            };

            Response.Cookies.Append("jwt", token, cookieOptions);

            Response.Headers.Append("Access-Control-Allow-Credentials", "true");
            Response.Headers.Append("Access-Control-Allow-Origin",
                Request.Headers["Origin"].FirstOrDefault() ?? "http://localhost:5125");

            // Determine redirect URL based on role


            return Ok(new
            {
                message = "Login successful.",
                role = user.Role,

            });
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new { message = "Logged out successfully." });
        }


        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }

        private string GenerateOtp()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.UserId.ToString())

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}