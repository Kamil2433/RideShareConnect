using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Models
{
    public class DriverProfileModel
    {
        [Key]
        public int DriverProfileId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required, StringLength(20)]
        public string LicenseNumber { get; set; } = string.Empty;

        [Required]
        public DateTime LicenseExpiryDate { get; set; }

        [StringLength(255)]
        public string LicenseImageUrl { get; set; } = string.Empty;

        [Range(0, 50)]
        public int YearsOfExperience { get; set; }

        [Required, StringLength(100)]
        public string EmergencyContactName { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string EmergencyContactPhone { get; set; } = string.Empty;

        public bool IsVerified { get; set; } = false;

        public DateTime? VerifiedAt { get; set; }

        // Relationships
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public ICollection<DriverRating> DriverRatings { get; set; } = new List<DriverRating>();
    }
}
