using System;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Dtos
{
    public class RideBookingCreateDto
    {
        [Required]
        public int RideId { get; set; }

        [Required]
        public int PassengerId { get; set; }

        [Required]
        public int SeatsBooked { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string BookingStatus { get; set; } = "Pending";

        public string PickupPoint { get; set; }
        public string DropPoint { get; set; }
        public string PassengerNotes { get; set; }
    }
}
