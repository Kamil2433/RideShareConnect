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

        public async Task<string?> GetPassengerEmailByBookingIdAsync(int bookingId)
        {
            return await _context.RideBookings
                .Where(b => b.BookingId == bookingId)
                .Join(_context.Users,
                    booking => booking.PassengerId,
                    user => user.UserId,
                    (booking, user) => user.Email)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ApproveOrRejectBookingAsync(int bookingId, int driverId, bool isApproved)
        {
            var booking = await _context.RideBookings
                .Include(b => b.Ride)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId && b.Ride.DriverId == driverId);

            if (booking == null)
                return false;

            booking.BookingStatus = isApproved ? "Confirmed" : "Rejected";

            _context.RideBookings.Update(booking);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<IEnumerable<(Ride Ride, string DriverFirstName)>> SearchRidesAsync(RideSearchDto searchDto)
        {
            var originLower = searchDto.Origin.ToLower();
            var destinationLower = searchDto.Destination.ToLower();
            var searchDate = searchDto.DepartureDate.Date;

            var query = from ride in _context.Rides
                        join user in _context.Users on ride.DriverId equals user.UserId
                        join profile in _context.UserProfiles on user.UserId equals profile.UserId
                        where ride.DepartureTime.Date == searchDate &&
                              ride.Status == "Scheduled" &&
                              ride.AvailableSeats > 0 &&
                              _context.RoutePoints.Any(o =>
                                  o.RideId == ride.RideId &&
                                  o.LocationName.ToLower().Contains(originLower)) &&
                              _context.RoutePoints.Any(d =>
                                  d.RideId == ride.RideId &&
                                  d.LocationName.ToLower().Contains(destinationLower) &&
                                  d.SequenceOrder >
                                      _context.RoutePoints
                                          .Where(o => o.RideId == ride.RideId &&
                                                     o.LocationName.ToLower().Contains(originLower))
                                          .Select(o => o.SequenceOrder)
                                          .FirstOrDefault())
                        select new { Ride = ride, DriverFirstName = profile.FirstName };

            var results = await query.ToListAsync();

            return results.Select(x => (x.Ride, x.DriverFirstName));
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

        public async Task<IEnumerable<RideBooking>> GetBookingsByDriverIdWithStatusAsync(int driverId, List<string> statuses)
        {
            return await _context.RideBookings
                .Include(rb => rb.Ride)
                .Where(rb => rb.Ride.DriverId == driverId && statuses.Contains(rb.BookingStatus))
                .ToListAsync();
        }


          public async Task<IEnumerable<RideBooking>> GetBookingsByPassengerIdWithStatusAsync(int PassId, List<string> statuses)
        {
            return await _context.RideBookings
                .Include(rb => rb.Ride)
                .Where(rb => rb.PassengerId == PassId && statuses.Contains(rb.BookingStatus))
                .ToListAsync();
        }


    }
}
