using RideShareConnect.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RideShareConnect.Services
{
        public interface IRideService
        {

                Task<string?> GetPassengerEmailByBookingIdAsync(int bookingId);


                Task<bool> ApproveOrRejectBookingAsync(int bookingId, int driverId, bool isApproved);

                Task<IEnumerable<RideBookingDto>> GetDriverRideBookingsByStatusAsync(int driverId);

                Task<IEnumerable<RideBookingDto>> GetDriverRideBookingsByStatusAsyncPass(int PassId);

                Task<bool> CreateRideAsync(RideCreateDto dto, int driverId);
                Task<IEnumerable<RideDto>> SearchRidesAsync(RideSearchDto searchDto);

                Task<bool> BookRideAsync(RideBookingCreateDto dto, int passengerId);
                Task<IEnumerable<RideDto>> GetRidesByUserIdAsync(int userId);
        }
}
