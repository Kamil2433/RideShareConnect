using System;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Dtos
{
    public class RideSearchDto
    {
        [Required]
        [StringLength(100)]
        public string Origin { get; set; }

        [Required]
        [StringLength(100)]
        public string Destination { get; set; }

        [Required]
        public DateTime DepartureDate { get; set; }
    }
}
