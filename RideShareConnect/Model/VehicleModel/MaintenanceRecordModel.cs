using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RideShareConnect.Models
{
	public class MaintenanceRecordModel
	{
		[Key]
		public int MaintenanceId { get; set; }

		[Required]
		public int VehicleId { get; set; }

		[Required, StringLength(50)]
		public string MaintenanceType { get; set; } = string.Empty;

		[StringLength(500)]
		public string Description { get; set; } = string.Empty;

		[Required, Range(0, 100000)]
		public decimal Cost { get; set; }

		[Required]
		public DateTime MaintenanceDate { get; set; }

		public DateTime? NextDueDate { get; set; }

		[StringLength(100)]
		public string ServiceProvider { get; set; } = string.Empty;

		// Navigation
		[ForeignKey("VehicleId")]
		public Vehicle? Vehicle { get; set; }
	}
}
