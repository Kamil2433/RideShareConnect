using System;
using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Dtos
{

    public class RoutePointDto
    {
        public string LocationName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int SequenceOrder { get; set; }
        public DateTime? EstimatedTime { get; set; }
        public bool IsPickupPoint { get; set; }
        public bool IsDropPoint { get; set; }
    }

}