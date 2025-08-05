using RideShareConnect.Data;
using RideShareConnect.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RideShareConnect.Dtos;
using RideShareConnect.Models;
using RideShareConnect.Repository.Interfaces;

namespace RideShareConnect.Repository.Implements
{
    public class RideRepository : IRideRepository
    {
        private readonly AppDbContext _context;

        public RideRepository(AppDbContext context)
        {
            _context = context;
        }



       public async Task<IEnumerable<Ride>> SearchRidesAsync(RideSearchDto searchDto)
{
    var originLower = searchDto.Origin.ToLower();
    var destinationLower = searchDto.Destination.ToLower();
    var searchDate = searchDto.DepartureDate.Date;

    var matchingRides = await _context.Rides
        .Where(r =>
            r.DepartureTime.Date == searchDate &&
            r.Status == "Scheduled" &&
            r.AvailableSeats > 0 &&
            _context.RoutePoints.Any(o =>
                o.RideId == r.RideId &&
                o.LocationName.ToLower().Contains(originLower)) &&
            _context.RoutePoints.Any(d =>
                d.RideId == r.RideId &&
                d.LocationName.ToLower().Contains(destinationLower) &&
                d.SequenceOrder >
                    _context.RoutePoints
                        .Where(o => o.RideId == r.RideId && o.LocationName.ToLower().Contains(originLower))
                        .Select(o => o.SequenceOrder)
                        .FirstOrDefault()
            )
        )
        .Include(r => r.RoutePoints)
        .ToListAsync();

    return matchingRides;
}





        public async Task<bool> CreateRideAsync(Ride ride)
        {
            ride.CreatedAt = DateTime.UtcNow;
            ride.UpdatedAt = DateTime.UtcNow;

            _context.Rides.Add(ride);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task AddRoutePointsAsync(IEnumerable<RoutePoint> routePoints)
        {
            await _context.RoutePoints.AddRangeAsync(routePoints);
            await _context.SaveChangesAsync();
        }



        public async Task<bool> CreateRideBookingAsync(RideBooking booking)
        {
            _context.RideBookings.Add(booking);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<IEnumerable<Ride>> GetRidesByUserIdAsync(int userId)
        {
            return await _context.Rides
                .Where(r => r.DriverId == userId)
                .ToListAsync();
        }


    }
}
