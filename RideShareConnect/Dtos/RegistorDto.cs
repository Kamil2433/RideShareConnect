using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Dtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } // "Driver", "Passenger", "Admin"

        [Required]
        public bool TermsAccepted { get; set; }
    }

    public class RegisterResponseDto
    {
        public string Message { get; set; }
    }
}