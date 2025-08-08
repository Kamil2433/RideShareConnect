using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RideShareConnect.Models.Admin
{
    public class Complaints
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int ReportedUserId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int ReportingUserId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Reason { get; set; }

        [Required]
        public string Status { get; set; } // Pending, Resolved

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
