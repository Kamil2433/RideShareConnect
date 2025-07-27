using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Models
{
    public class RideBooking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int RideId { get; set; } // FK to Ride

        [Required]
        public int PassengerId { get; set; } // FK to User

        [Required]
        public int SeatsBooked { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [StringLength(50)]
        public string BookingStatus { get; set; } // "Confirmed", "Cancelled", "Pending"

        public DateTime BookingTime { get; set; }

        public DateTime? CancelledAt { get; set; }
        public string CancellationReason { get; set; }

        [StringLength(255)]
        public string PickupPoint { get; set; }

        [StringLength(255)]
        public string DropPoint { get; set; }

        public string PassengerNotes { get; set; }

        // Navigation Properties
        public Ride Ride { get; set; }
        public ICollection<BookingHistory> BookingHistories { get; set; }
    }
}
