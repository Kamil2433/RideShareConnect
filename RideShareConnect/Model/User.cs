using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RideShareConnect.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        // Authentication & Account Info
        [Required]
        public string Email { get; set; } // Required, non-nullable
        [Required]
        public string PhoneNumber { get; set; } // Required, non-nullable
        [Required]
        public string PasswordHash { get; set; } // Required, non-nullable
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Default value
        public DateTime UpdatedAt { get; set; } = DateTime.Now; // Default value
        public bool IsActive { get; set; } = true; // Default value
        [Required]
        public string Role { get; set; } // Required, non-nullable
        public bool IsEmailVerified { get; set; } = false; // Default value
        public bool IsPhoneVerified { get; set; } = false; // Default value
        
        // Profile Information (Merged from UserProfile)
        [Required]
        public string FirstName { get; set; } // Required, non-nullable
        [Required]
        public string LastName { get; set; } // Required, non-nullable
        public string? ProfilePicture { get; set; } // Nullable
        public DateTime? DateOfBirth { get; set; } // Nullable
        public string? Gender { get; set; } // Nullable
        public string? Address { get; set; } // Nullable
        public string? City { get; set; } // Nullable
        public string? State { get; set; } // Nullable
        public string? Country { get; set; } // Nullable
        public string? Bio { get; set; } // Nullable
        public decimal Rating { get; set; } = 0.0m; // Default value
    }
}