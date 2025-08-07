using System;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Models.Admin
{
    public class Commission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; }

        [Required]
        [StringLength(500)]
        public string Value { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
