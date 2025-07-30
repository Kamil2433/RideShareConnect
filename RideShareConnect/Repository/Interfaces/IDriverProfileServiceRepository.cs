using RideShareConnect.Models;
using System.Threading.Tasks;

namespace RideShareConnect.Repository.Interfaces
{
	public interface IDriverProfileServiceRepository
    {
		Task CreateDriverProfile(DriverProfile profile);
		Task UpdateDriverProfile(int id, DriverProfile profile);
		Task VerifyDriver(int id);
		Task<bool> CheckLicenseExpiry(int id);
	}
}
