using System;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }

        [Required]
        public int DriverId { get; set; }

        // Fields shown in the form
        [Required, StringLength(30)]
        public string VehicleType { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string LicensePlate { get; set; } = string.Empty; // Matches VehicleNumber in form

        [Required, StringLength(50)]
        public string InsuranceNumber { get; set; } = string.Empty;

        [Required]
        public DateTime RegistrationExpiry { get; set; }

        // Document storage (required for form)
        [Required]
        public string RCDocumentBase64 { get; set; } // Stores RC document image

        [Required]
        public string InsuranceDocumentBase64 { get; set; } // Stores insurance document image

        // Approval status (new field)
        public bool IsApproved { get; set; } = false; // Default to false (pending approval)

        // Existing fields (kept but not shown in form)
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}