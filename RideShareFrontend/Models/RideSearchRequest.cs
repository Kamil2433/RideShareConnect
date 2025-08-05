using System;
using System.ComponentModel.DataAnnotations;

namespace RideShareFrontend.DTOs
{
    public class RideSearchRequestDto
    {
        [Required(ErrorMessage = "Origin is required")]
        [StringLength(100, ErrorMessage = "Origin cannot exceed 100 characters")]
        [Display(Name = "Departure From")]
        public string Origin { get; set; } = string.Empty;

        [Required(ErrorMessage = "Destination is required")]
        [StringLength(100, ErrorMessage = "Destination cannot exceed 100 characters")]
        [Display(Name = "Arrival At")]
        public string Destination { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Travel Date")]
        [FutureDate(ErrorMessage = "Date must be today or in the future")]
        public DateTime RideDate { get; set; } = DateTime.Today;

        [Range(1, 10, ErrorMessage = "Seats must be between 1 and 10")]
        [Display(Name = "Passengers")]
        public int Passengers { get; set; } = 1;

        [Display(Name = "Flexible Dates (±1 day)")]
        public bool FlexibleDates { get; set; } = false;
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is DateTime date && date >= DateTime.Today;
        }
    }
}