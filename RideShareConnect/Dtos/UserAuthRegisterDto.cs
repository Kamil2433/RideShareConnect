using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Dtos
{
    public class UserAuthRegisterDto
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } // "Driver", "Passenger", "Admin"
    }

    public class UserAuthRegisterResponseDto
    {
        public string Message { get; set; }
    }

    public class UserAuthVerifyEmailDto
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Code { get; set; }
    }

    public class UserAuthLoginDto
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }

    public class UserAuthLoginResponseDto
    {
        public string Message { get; set; }
        public string Token { get; set; }
    }
}