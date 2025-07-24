using RideShareConnect.Models;
using System.Threading.Tasks;

namespace RideShareConnect.Repository.Interfaces
{
	public interface IDriverProfileServiceRepository
    {
		Task CreateDriverProfile(DriverProfileModel profile);
		Task UpdateDriverProfile(int driverProfileId,DriverProfileModel profile);
		Task VerifyDriver(int driverProfileId);
		Task<bool> CheckLicenseExpiry(int driverProfileId);
	}
}
