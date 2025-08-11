using AutoMapper;
using RideShareConnect.Dtos;
using RideShareConnect.Models;

namespace RideShareConnect.MappingProfiles
{
    public class ResetPasswordMapperProfile : Profile
    {
        public ResetPasswordMapperProfile()
        {
            CreateMap<VerifyResetPasswordOtpDto, ResetPasswordOtp>();
            CreateMap<ResetPasswordOtp, VerifyResetPasswordOtpDto>();

            CreateMap<ResetPasswordDto, User>();
            CreateMap<User, ResetPasswordDto>();
        }
    }
}
