using AutoMapper;
using RideShareConnect.Dtos;
using RideShareConnect.Models;

namespace RideShareConnect.MappingProfiles
{
    public class UserProfileAutoMapperProfile : Profile
    {
        public UserProfileAutoMapperProfile()
        {
            CreateMap<UserProfileDto, UserProfile>();
            CreateMap<UserProfile, UserProfileDto>();
        }
    }
}