using RideShareConnect.Models;
using RideShareConnect.Data;
using RideShareConnect.Repository.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;   
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;


namespace RideShareConnect.Repository.Implements
{
    public class MaintenanceRecordService : IMaintenanceRecordRepository
    {
        private readonly AppDbContext _context;
        public MaintenanceRecordService(AppDbContext context) => _context = context;

        public async Task RecordMaintenance(MaintenanceRecord record)
        {
            await _context.MaintenanceRecords.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task ScheduleNextMaintenance(int vehicleId, DateTime nextDueDate)
        {
            var lastRecord = await _context.MaintenanceRecords
                .Where(m => m.VehicleId == vehicleId)
                .OrderByDescending(m => m.MaintenanceDate)
                .FirstOrDefaultAsync();

            if (lastRecord != null)
            {
                lastRecord.NextDueDate = nextDueDate;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MaintenanceRecord>> GetMaintenanceHistory(int vehicleId)
        {
            return await _context.MaintenanceRecords
                .Where(m => m.VehicleId == vehicleId)
                .ToListAsync();
        }
    }
}
