// using RideShareConnect.Models;
// using RideShareConnect.Data;
// using RideShareConnect.Repository.Interfaces;

// using System.Threading.Tasks;
// using System.Collections.Generic;
// using Microsoft.EntityFrameworkCore;
// using System.Linq;
// using System;



// namespace RideShareConnect.Repository.Implements
// {

//     public class DriverRatingService : IDriverRatingRepository
//     {
//         private readonly AppDbContext _context;
//         public DriverRatingService(AppDbContext context) => _context = context;

//         public async Task RateDriver(DriverRating rating)
//         {
//             await _context.DriverRatings.AddAsync(rating);
//             await _context.SaveChangesAsync();
//         }

//         public async Task<double> CalculateAverageRating(int driverId)
//         {
//             var ratings = await _context.DriverRatings
//                 .Where(r => r.DriverId == driverId)
//                 .ToListAsync();
//             return ratings.Count == 0 ? 0 : ratings.Average(r => r.Rating);
//         }

//         public async Task<IEnumerable<DriverRating>> GetDriverRatings(int driverId)
//         {
//             return await _context.DriverRatings
//                 .Where(r => r.DriverId == driverId)
//                 .ToListAsync();
//         }
//     }
// }