using RideShareConnect.Models;
using System.Threading.Tasks;

namespace RideShareConnect.Repository.Interfaces
{
	public interface IVehicleDocumentServiceRepository
	{
		Task UploadDocument(VehicleDocument doc);
		Task VerifyDocument(int id);
		Task<bool> CheckExpiry(int vehicleID);
		Task<int> RenewDocument(int id);
	}
}
