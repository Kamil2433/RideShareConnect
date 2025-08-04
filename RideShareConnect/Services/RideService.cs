using RideShareConnect.Dtos;
using RideShareConnect.Models;
using RideShareConnect.Repository.Implements;
using RideShareConnect.Repository.Interfaces;
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

            // First save the ride to get the ID
            var result = await _rideRepository.CreateRideAsync(ride);

            if (result)
            {
                var routePoints = new List<RoutePoint>();
                int sequenceOrder = 1;

                // Add origin as the first route point
                routePoints.Add(new RoutePoint
                {
                    RideId = ride.RideId,
                    LocationName = dto.Origin,
                    Latitude = 0, // Optional: Fill if you have coordinates
                    Longitude = 0,
                    SequenceOrder = sequenceOrder++,
                    EstimatedTime = dto.DepartureTime,
                    IsPickupPoint = true,
                    IsDropPoint = false,
                
                });


                if (!string.IsNullOrWhiteSpace(dto.RoutePoints))
                {
                    var points = dto.RoutePoints.Split(',')
                        .Select(p => p.Trim())
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .ToList();

                    foreach (var point in points)
                    {
                        routePoints.Add(new RoutePoint
                        {
                            RideId = ride.RideId,
                            LocationName = point,
                            Latitude = 0, // You should implement geocoding here
                            Longitude = 0,
                            SequenceOrder = sequenceOrder++,
                            EstimatedTime = dto.DepartureTime.AddMinutes(sequenceOrder * 10), // Example estimation
                            IsPickupPoint = false,
                            IsDropPoint = false,
                        
                        });
                    }
                }



                // Add destination as the last route point
                routePoints.Add(new RoutePoint
                {
                    RideId = ride.RideId,
                    LocationName = dto.Destination,
                    Latitude = 0, // Optional: Fill if you have coordinates
                    Longitude = 0,
                    SequenceOrder = sequenceOrder,
                    EstimatedTime = dto.ArrivalTime,
                    IsPickupPoint = false,
                    IsDropPoint = true,
                  
                });

                await _rideRepository.AddRoutePointsAsync(routePoints);
            }

            return result;
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
