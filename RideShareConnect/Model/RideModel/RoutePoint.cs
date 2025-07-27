using System;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Models
{
    public class RoutePoint
    {
        [Key]
        public int RoutePointId { get; set; }

        [Required]
        public int RideId { get; set; }  // FK to Ride

        [Required]
        [StringLength(255)]
        public string LocationName { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        public int SequenceOrder { get; set; }

        public DateTime EstimatedTime { get; set; }

        public bool IsPickupPoint { get; set; }
        public bool IsDropPoint { get; set; }

        // Navigation Property
        public Ride Ride { get; set; }
    }
}
