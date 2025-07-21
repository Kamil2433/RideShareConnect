// File: Models/User.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; }

        [Required]
        public bool IsEmailVerified { get; set; }

        public bool IsPhoneVerified { get; set; }
    }
}