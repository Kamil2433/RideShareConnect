using AutoMapper;
using RideShareConnect.Dtos;
using RideShareConnect.Models;

namespace RideShareConnect.MappingProfiles
{


    public class RideAndBookingProfile : Profile
    {
        public RideAndBookingProfile()
        {
            CreateMap<Ride, RideDto>().ReverseMap();
            //CreateMap<Ride, RideDto>().ReverseMap();

            //CreateMap<RideBooking, RideBookingDto>().ReverseMap();
            //CreateMap<RoutePoint, RoutePointDto>().ReverseMap();
        }
    }


}