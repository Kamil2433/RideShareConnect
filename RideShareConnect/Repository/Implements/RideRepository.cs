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
