using RideShareConnect.Models;
using System.Threading.Tasks;

namespace RideShareConnect.Repository.Interfaces
{
	public interface IDriverProfileServiceRepository
    {
		Task CreateDriverProfile(DriverProfile profile);
		Task UpdateDriverProfile(int driverProfileId,DriverProfile profile);
		Task VerifyDriver(int driverProfileId);
		Task<bool> CheckLicenseExpiry(int driverProfileId);
	}
}
