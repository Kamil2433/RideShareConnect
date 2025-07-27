using RideShareConnect.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RideShareConnect.Repositories
{
	public interface IRideRepository
	{
        Task<bool> CreateRideBookingAsync(RideBooking booking);
        Task<IEnumerable<Ride>> GetRidesByUserIdAsync(int userId);
	}
}
