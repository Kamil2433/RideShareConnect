using AutoMapper;
using RideShareConnect.Dtos;
using RideShareConnect.Models;

namespace RideShareConnect.MappingProfiles
{
    public class Module1AutoMapperProfile : Profile
    {
        public Module1AutoMapperProfile()
        {
            CreateMap<UserAuthRegisterDto, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.IsEmailVerified, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
        
    }
}