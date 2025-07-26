using RideShareConnect.Models;
using System.Threading.Tasks;

namespace RideShareConnect.Repository.Interfaces
{
	public interface IVehicleDocumentServiceRepository
	{
		Task UploadDocument(VehicleDocument vehicleDocument);
		Task VerifyDocument(int documentId );
		Task<bool> CheckExpiry(int vehicleID);
		Task<int> RenewDocument(int documentId);
	}
}
