using System;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Models.Admin
{
    public class Analytics
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int TotalUsers { get; set; }

        [Required]
        public int TotalRides { get; set; }

        [Required]
        public decimal TotalRevenue { get; set; }

        public int NewRegistrations { get; set; }

        public int ReportsFiled { get; set; }
    }
}
