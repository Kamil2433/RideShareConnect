
// using RideShareConnect.Models;
// using RideShareConnect.Data;
// using RideShareConnect.Repository.Interfaces;
// using System.Threading.Tasks;
// using System.Collections.Generic;
// using Microsoft.EntityFrameworkCore;
// using System.Linq;
// using System;

// namespace RideShareConnect.Repository.Implements
// {
// 	public class VehicleDocumentService : IVehicleDocumentServiceRepository
// 	{
// 		private readonly AppDbContext _context;
// 		public VehicleDocumentService(AppDbContext context) => _context = context;

// 		public async Task UploadDocument(VehicleDocument doc)
// 		{
// 			await _context.VehicleDocuments.AddAsync(doc);
// 			await _context.SaveChangesAsync();
// 		}

// 		public async Task VerifyDocument(int id)
// 		{
// 			var doc = await _context.VehicleDocuments.FindAsync(id);
// 			if (doc != null)
// 			{
// 				doc.Status = "Verified";
// 				doc.VerifiedAt = DateTime.UtcNow;
// 				await _context.SaveChangesAsync();
// 			}
// 		}

// 		public async Task<bool> CheckExpiry(int vehicleId)
// 		{
// 			var expired = await _context.VehicleDocuments
// 				.Where(d => d.VehicleId == vehicleId && d.ExpiryDate < DateTime.UtcNow)
// 				.ToListAsync();
// 			return expired.Any();
// 		}

// 		public async Task<int> RenewDocument(int id)
// 		{
// 			var doc = await _context.VehicleDocuments.FindAsync(id);
// 			if (doc != null)
// 			{
// 				doc.ExpiryDate = doc.ExpiryDate.AddYears(1);
// 				await _context.SaveChangesAsync();
// 			}
// 			return id;
// 		}
// 	}
// }
