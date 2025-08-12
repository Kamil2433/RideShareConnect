using System.Threading.Tasks;
using RideShareConnect.Dtos;
using System.Threading.Tasks;

namespace RideShareConnect.Repository.Interfaces
{
    public interface IResetPasswordRepository
    {
        Task<bool> SendResetPasswordOtpAsync(SendResetPasswordOtpDto dto);
        Task<bool> VerifyOtpAsync(VerifyResetPasswordOtpDto dto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    }
}
