using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RideShareConnect.Models
{
    public class UserVerification
    {
        [Key]
        public int VerificationId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string DocumentType { get; set; }

        [StringLength(100)]
        public string DocumentNumber { get; set; }

        [StringLength(255)]
        public string DocumentImageUrl { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [Required]
        public DateTime SubmittedAt { get; set; }

        public DateTime? VerifiedAt { get; set; }

        [StringLength(100)]
        public string VerifiedBy { get; set; }

        [StringLength(500)]
        public string Comments { get; set; }

        public User User { get; set; }
    }
}