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

        [Required, StringLength(50)]
        public string Make { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Model { get; set; } = string.Empty;

        [Required, Range(1900, 2100)]
        public int Year { get; set; }

        [Required, StringLength(30)]
        public string Color { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string LicensePlate { get; set; } = string.Empty;

        [Required, Range(1, 10)]
        public int SeatingCapacity { get; set; }

        [Required, StringLength(30)]
        public string VehicleType { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Relationships
        public ICollection<VehicleDocument> VehicleDocuments { get; set; } = new List<VehicleDocument>();
        public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();
    }
}
