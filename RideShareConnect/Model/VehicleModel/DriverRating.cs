using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RideShareConnect.Models
{
	public class DriverRating
	{
		[Key]
		public int RatingId { get; set; }

		[Required]
		public int DriverId { get; set; }

		[Required]
		public int PassengerId { get; set; }

		[Required]
		public int RideId { get; set; }

		[Required, Range(1, 5)]
		public int Rating { get; set; }

		[StringLength(500)]
		public string Comment { get; set; } = string.Empty;

		[Required]
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		// Navigation
		// [ForeignKey("DriverId")]
		// public DriverProfile? Driver { get; set; }
	}
}
