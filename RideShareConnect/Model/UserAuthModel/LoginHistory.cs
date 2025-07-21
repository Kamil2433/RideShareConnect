using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RideShareConnect.Models
{
    public class LoginHistory
    {
        [Key]
        public int LoginId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public DateTime LoginTime { get; set; }

        [StringLength(45)]
        public string IPAddress { get; set; }

        [StringLength(255)]
        public string UserAgent { get; set; }

        [StringLength(50)]
        public string DeviceType { get; set; }

        [Required]
        public bool IsSuccessful { get; set; }

        public User User { get; set; }
    }
}
