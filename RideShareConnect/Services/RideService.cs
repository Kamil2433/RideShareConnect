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


        public async Task<IEnumerable<RideDto>> SearchRidesAsync(RideSearchDto searchDto)
        {
            var rides = await _rideRepository.SearchRidesAsync(searchDto);

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


        public async Task<bool> CreateRideAsync(RideCreateDto dto, int driverId)
        {
            var ride = new Ride
            {
                DriverId = driverId,
                VehicleId = dto.VehicleId,
                Origin = dto.Origin,
                Destination = dto.Destination,
                DepartureTime = dto.DepartureTime,
                ArrivalTime = dto.ArrivalTime,
                AvailableSeats = dto.AvailableSeats,
                BookedSeats = dto.BookedSeats,
                PricePerSeat = dto.PricePerSeat,
                RideType = dto.RideType ?? "OneTime",
                Status = dto.Status ?? "Scheduled",
                Notes = dto.Notes,
                IsRecurring = dto.IsRecurring,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return await _rideRepository.CreateRideAsync(ride);
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
