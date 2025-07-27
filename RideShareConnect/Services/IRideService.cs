using RideShareConnect.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RideShareConnect.Services
{
	public interface IRideService
	{
        Task<bool> BookRideAsync(RideBookingCreateDto dto, int passengerId);
        Task<IEnumerable<RideDto>> GetRidesByUserIdAsync(int userId);
	}
}
