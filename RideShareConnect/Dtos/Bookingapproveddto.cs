
using System;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Dtos
{
    public class ApproveBookingDto
    {
        public int BookingId { get; set; }
        public bool IsApproved { get; set; }
    }

}