using System;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Models
{
    public class RideRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        public int PassengerId { get; set; } // FK to User

        [Required]
        [StringLength(255)]
        public string Origin { get; set; }

        [Required]
        [StringLength(255)]
        public string Destination { get; set; }

        public DateTime PreferredTime { get; set; }

        [Required]
        public int SeatsNeeded { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MaxPrice { get; set; }

        [StringLength(50)]
        public string Status { get; set; } // "Open", "Matched", "Cancelled"

        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
