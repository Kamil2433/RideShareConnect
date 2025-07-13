using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RideShareConnect.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; }

        [Required, MaxLength(100)]
        public string PasswordHash { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public string? ProfileImageUrl { get; set; }

        public bool IsDriver { get; set; }
        public bool IsPassenger { get; set; }

        public bool IsVerified { get; set; } = false;
        public bool IsSuspended { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
