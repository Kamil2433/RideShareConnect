using RideShareConnect.Data;
using RideShareConnect.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RideShareConnect.Dtos;
using RideShareConnect.Models;
using RideShareConnect.Repositories;

namespace RideShareConnect.Repositories
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
            return await _context.Rides
                .Where(r =>
                    r.Origin.ToLower() == searchDto.Origin.ToLower() &&
                    r.Destination.ToLower() == searchDto.Destination.ToLower() &&
                    r.DepartureTime.Date == searchDto.DepartureDate.Date &&
                    r.Status == "Scheduled" &&
                    r.AvailableSeats > 0)
                .ToListAsync();
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
