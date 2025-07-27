// Dtos/RideCreateDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Dtos
{
    public class RideCreateDto
    {
        [Required]
        public int DriverId { get; set; }

        [Required]
        public int VehicleId { get; set; }

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

        public string RideType { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        public bool IsRecurring { get; set; }
    }
}
