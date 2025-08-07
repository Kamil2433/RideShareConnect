using System;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Models.Admin
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        public int AdminId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
