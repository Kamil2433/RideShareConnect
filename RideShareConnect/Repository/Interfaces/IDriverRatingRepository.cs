using RideShareConnect.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RideShareConnect.Repository.Interfaces
{
    public interface IDriverRatingRepository
    {
        Task RateDriver(DriverRating rating);
        Task<double> CalculateAverageRating(int driverId);
        Task<IEnumerable<DriverRating>> GetDriverRatings(int driverId);
    }
}
