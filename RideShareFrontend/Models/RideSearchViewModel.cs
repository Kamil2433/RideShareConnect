using System;

namespace RideShareFrontend.DTOs
{
    public class RideSearchResultDto
    {
        public string RideId { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        
        [Display(Name = "Departure")]
        public DateTime DepartureTime { get; set; }
        
        [Display(Name = "Arrival")]
        public DateTime EstimatedArrivalTime { get; set; }
        
        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PricePerSeat { get; set; }
        
        [Display(Name = "Seats Available")]
        public int AvailableSeats { get; set; }
        
        [Display(Name = "Vehicle")]
        public string VehicleInfo { get; set; } = string.Empty;
        
        [Display(Name = "Driver")]
        public string DriverName { get; set; } = string.Empty;
        
        [Display(Name = "Rating")]
        [DisplayFormat(DataFormatString = "{0:N1}")]
        public decimal DriverRating { get; set; }
        
        public bool IsVerifiedDriver { get; set; }
        public bool HasAirConditioning { get; set; }
        public string[] Amenities { get; set; } = Array.Empty<string>();
    }
}