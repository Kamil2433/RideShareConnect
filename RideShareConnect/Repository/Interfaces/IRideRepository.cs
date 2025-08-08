using RideShareConnect.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using RideShareConnect.Dtos;

namespace RideShareConnect.Repository.Interfaces
{
    public interface IRideRepository
    {

        Task<IEnumerable<RideBooking>> GetBookingsByDriverIdWithStatusAsync(int driverId, List<string> statuses);

        Task<IEnumerable<Ride>> SearchRidesAsync(RideSearchDto searchDto);
        Task<bool> CreateRideAsync(Ride ride);
        Task AddRoutePointsAsync(IEnumerable<RoutePoint> routePoints);
        Task<bool> CreateRideBookingAsync(RideBooking booking);
        Task<IEnumerable<Ride>> GetRidesByUserIdAsync(int userId);
    }
}