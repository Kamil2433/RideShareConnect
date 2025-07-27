using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Models
{
    public class Ride
    {
        [Key]
        public int RideId { get; set; }

        [Required]
        public int DriverId { get; set; }  // FK to User

        [Required]
        public int VehicleId { get; set; } // FK to Vehicle

        [Required]
        [StringLength(255)]
        public string Origin { get; set; }

        [Required]
        [StringLength(255)]
        public string Destination { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        [Required]
        public int AvailableSeats { get; set; }

        [Required]
        public int BookedSeats { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal PricePerSeat { get; set; }

        [StringLength(50)]
        public string RideType { get; set; } // "OneTime", "Recurring"

        [StringLength(50)]
        public string Status { get; set; } // "Scheduled", "Completed", "Cancelled"

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsRecurring { get; set; }

        // Navigation Properties
        public ICollection<RoutePoint> RoutePoints { get; set; }
        public ICollection<RideBooking> RideBookings { get; set; }
    }
}
