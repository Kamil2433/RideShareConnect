using System;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Models
{
    public class BookingHistory
    {
        [Key]
        public int HistoryId { get; set; }

        [Required]
        public int BookingId { get; set; } // FK to RideBooking

        [StringLength(50)]
        public string StatusFrom { get; set; }

        [StringLength(50)]
        public string StatusTo { get; set; }

        public DateTime ChangedAt { get; set; }

        [StringLength(255)]
        public string ChangedBy { get; set; } // UserName or AdminId

        public string Reason { get; set; }

        public string Notes { get; set; }

        // Navigation Property
        public RideBooking RideBooking { get; set; }
    }
}
