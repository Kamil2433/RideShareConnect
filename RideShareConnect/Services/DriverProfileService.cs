using RideShareConnect.Dtos;
using RideShareConnect.Models;
using RideShareConnect.Repository.Interfaces;
using RideShareConnect.Services.Interfaces;

namespace RideShareConnect.Services.Implements
{
    public class DriverProfileService : IDriverProfileService
    {
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly IDriverProfileRepository _driverProfileRepo;

        public DriverProfileService(
            IUserProfileRepository userProfileRepo,
            IDriverProfileRepository driverProfileRepo)
        {
            _userProfileRepo = userProfileRepo;
            _driverProfileRepo = driverProfileRepo;
        }

        public async Task<DriverProfileDto> GetDriverProfileAsync(int userId)
        {
            var userProfile = await _userProfileRepo.GetProfileByUserIdAsync(userId);
            var driverProfile = await _driverProfileRepo.GetByUserIdAsync(userId);


            if (userProfile == null) return null;

            var dto = MapToDriverProfileDto(userProfile, driverProfile);
            return dto;
        }

        public async Task<bool> CreateOrUpdateDriverProfileAsync(int userId, DriverProfileDto dto)
        {
            // Handle User Profile
            var userProfile = await _userProfileRepo.GetProfileByUserIdAsync(userId) ?? 
                            new UserProfile { UserId = userId };

            MapToUserProfile(dto, userProfile);

            var userSuccess = userProfile.ProfileId == 0 ? 
                await _userProfileRepo.CreateProfileAsync(userProfile) :
                await _userProfileRepo.UpdateProfileAsync(userId, userProfile);

            if (!userSuccess) return false;

            // Handle Driver Profile
            var driverProfile = await _driverProfileRepo.GetByUserIdAsync(userId) ?? 
                              new DriverProfile { UserId = userId };

            MapToDriverProfile(dto, driverProfile);

            var driverSuccess = driverProfile.DriverProfileId == 0 ? 
                await _driverProfileRepo.CreateAsync(driverProfile) :
                await _driverProfileRepo.UpdateAsync(driverProfile);

            return driverSuccess;
        }

        private DriverProfileDto MapToDriverProfileDto(UserProfile userProfile, DriverProfile driverProfile)
        {
            var dto = new DriverProfileDto
            {
                // User profile fields
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                PhoneNumber = userProfile.PhoneNumber,
                Address = userProfile.Address,
                ProfilePicture = userProfile.ProfilePicture,
                IsNewProfile = userProfile.ProfileId == 0,
            };

            if (driverProfile != null)
            {
                // Driver profile fields
                dto.LicenseNumber = driverProfile.LicenseNumber;
                dto.LicenseExpiryDate = driverProfile.LicenseExpiryDate;
                dto.LicenseImageUrl = driverProfile.LicenseImageUrl;
                dto.YearsOfExperience = driverProfile.YearsOfExperience;
                dto.EmergencyContactName = driverProfile.EmergencyContactName;
                dto.EmergencyContactPhone = driverProfile.EmergencyContactPhone;
                dto.isverfied=driverProfile.IsVerified;

            }


            return dto;
        }

        private void MapToUserProfile(DriverProfileDto dto, UserProfile userProfile)
        {
            userProfile.FirstName = dto.FirstName;
            userProfile.LastName = dto.LastName;
            userProfile.PhoneNumber = dto.PhoneNumber;
            userProfile.Address = dto.Address;
            userProfile.ProfilePicture = dto.ProfilePicture;
        }

        private void MapToDriverProfile(DriverProfileDto dto, DriverProfile driverProfile)
        {
            driverProfile.LicenseNumber = dto.LicenseNumber;
            driverProfile.LicenseExpiryDate = dto.LicenseExpiryDate;
            driverProfile.LicenseImageUrl = dto.LicenseImageUrl;
            driverProfile.YearsOfExperience = dto.YearsOfExperience;
            driverProfile.EmergencyContactName = dto.EmergencyContactName;
            driverProfile.EmergencyContactPhone = dto.EmergencyContactPhone;
        }
    }
}