using System;

namespace RideShareFrontend.Models.DTOs
{
    public class RideCreateDto
    {
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public decimal PricePerSeat { get; set; }
        public int AvailableSeats { get; set; }
    }

}
