using RideShareConnect.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using RideShareConnect.Dtos;

namespace RideShareConnect.Repositories
{
	public interface IRideRepository
	{

        Task<IEnumerable<Ride>> SearchRidesAsync(RideSearchDto searchDto);

        Task<bool> CreateRideAsync(Ride ride);

        Task<bool> CreateRideBookingAsync(RideBooking booking);
        Task<IEnumerable<Ride>> GetRidesByUserIdAsync(int userId);
	}
}
