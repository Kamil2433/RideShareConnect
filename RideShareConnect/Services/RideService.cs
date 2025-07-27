using RideShareConnect.Dtos;
using RideShareConnect.Models;
using RideShareConnect.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RideShareConnect.Services
{
    public class RideService : IRideService
    {
        private readonly IRideRepository _rideRepository;

        public RideService(IRideRepository rideRepository)
        {
            _rideRepository = rideRepository;
        }

        public async Task<bool> BookRideAsync(RideBookingCreateDto dto, int passengerId)
        {
            var booking = new RideBooking
            {
                RideId = dto.RideId,
                PassengerId = passengerId,
                SeatsBooked = dto.SeatsBooked,
                TotalAmount = dto.TotalAmount,
                PickupPoint = dto.PickupPoint,
                DropPoint = dto.DropPoint,
                PassengerNotes = dto.PassengerNotes,
                BookingStatus = "Confirmed",
                BookingTime = DateTime.UtcNow
            };



            return await _rideRepository.CreateRideBookingAsync(booking);
        }


        public async Task<IEnumerable<RideDto>> GetRidesByUserIdAsync(int userId)
        {
            var rides = await _rideRepository.GetRidesByUserIdAsync(userId);

            return rides.Select(ride => new RideDto
            {
                RideId = ride.RideId,
                DriverId = ride.DriverId,
                VehicleId = ride.VehicleId,
                Origin = ride.Origin,
                Destination = ride.Destination,
                DepartureTime = ride.DepartureTime,
                ArrivalTime = ride.ArrivalTime,
                AvailableSeats = ride.AvailableSeats,
                PricePerSeat = ride.PricePerSeat,
                RideType = ride.RideType,
                Status = ride.Status,
                Notes = ride.Notes,
                IsRecurring = ride.IsRecurring
            });
        }
    }
}
