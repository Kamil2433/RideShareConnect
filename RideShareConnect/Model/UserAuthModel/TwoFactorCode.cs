using System;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Models
{
    public class TwoFactorCode
    {
        [Key]
        public int CodeId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(6)]
        public string Code { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; }

        public User User { get; set; }
    }
}