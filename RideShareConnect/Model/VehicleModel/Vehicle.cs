using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }

        [Required]
        public int DriverId { get; set; }

        public DriverProfile Driver { get; set; }

        [Required, StringLength(30)]
        public string VehicleType { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string LicensePlate { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string InsuranceNumber { get; set; } = string.Empty;

        [Required]
        public DateTime RegistrationExpiry { get; set; }

        [Required]
        public string RCDocumentBase64 { get; set; }

        [Required]
        public string InsuranceDocumentBase64 { get; set; }

        public bool IsApproved { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

      
    }
}
