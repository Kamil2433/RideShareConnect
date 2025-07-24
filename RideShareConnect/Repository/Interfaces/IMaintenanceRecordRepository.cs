using RideShareConnect.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RideShareConnect.Repository.Interfaces
{
    public interface IMaintenanceRecordRepository
    {
        Task RecordMaintenance(MaintenanceRecordModel record);
        Task ScheduleNextMaintenance(int vehicleId, DateTime nextDueDate);
        Task<IEnumerable<MaintenanceRecord>> GetMaintenanceHistory(int vehicleId);
    }
}
