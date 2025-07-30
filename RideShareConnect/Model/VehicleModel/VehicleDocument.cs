using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RideShareConnect.Models
{
    public class VehicleDocument
    {
        [Key]
        public int DocumentId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required, StringLength(50)]
        public string DocumentType { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string DocumentNumber { get; set; } = string.Empty;

        [StringLength(255)]
        public string DocumentImageUrl { get; set; } = string.Empty;

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required, StringLength(20)]
        public string Status { get; set; } = "Pending";

        [Required]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public DateTime? VerifiedAt { get; set; }

        public virtual Vehicle Vehicle { get; set; } = null!;
    }
}
