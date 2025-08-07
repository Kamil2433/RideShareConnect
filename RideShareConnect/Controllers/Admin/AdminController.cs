        // using Microsoft.AspNetCore.Authorization;
        // using Microsoft.AspNetCore.Mvc;
        // using Microsoft.EntityFrameworkCore;
        // using RideShareConnect.Data;
        // using RideShareConnect.Models.Admin;
        // using RideShareConnect.Models;
        // using System.Security.Claims;

        // namespace RideShareConnect.Controllers.AdminModule
        // {
        //     [Route("api/admin")]
        //     [ApiController]
        //     [Authorize(Roles = "Admin")]
        //     public class AdminController : ControllerBase
        //     {
        //         private readonly AppDbContext _context;

        //         public AdminController(AppDbContext context)
        //         {
        //             _context = context;
        //         }

        //         // GET: /api/admin/dashboard
        //         [HttpGet("dashboard")]
        //         public async Task<IActionResult> GetDashboard()
        //         {
        //             var totalUsers = await _context.Users.CountAsync(u => u.Role == "Passenger");
        //             var totalDrivers = await _context.Users.CountAsync(u => u.Role == "Driver");
        //             var totalRides = await _context.Rides.CountAsync(); // Assuming you have Rides
        //             var totalRevenue = await _context.Rides.SumAsync(r => r.Fare); // Assuming Fare field

        //             return Ok(new
        //             {
        //                 TotalUsers = totalUsers,
        //                 TotalDrivers = totalDrivers,
        //                 TotalRides = totalRides,
        //                 TotalRevenue = totalRevenue
        //             });
        //         }

        //         // GET: /api/admin/users
        //         [HttpGet("users")]
        //         public async Task<IActionResult> GetAllUsers()
        //         {
        //             var users = await _context.Users
        //                 .Select(u => new
        //                 {
        //                     u.UserId,
        //                     u.Name,
        //                     u.Email,
        //                     u.Role,
        //                     u.IsEmailVerified,
        //                     u.IsActive
        //                 })
        //                 .ToListAsync();

        //             return Ok(users);
        //         }

        //         // PUT: /api/admin/users/{id}/status
        //         [HttpPut("users/{id}/status")]
        //         public async Task<IActionResult> UpdateUserStatus(int id, [FromBody] bool isActive)
        //         {
        //             var user = await _context.Users.FindAsync(id);
        //             if (user == null) return NotFound("User not found.");

        //             user.IsActive = isActive;
        //             user.UpdatedAt = DateTime.UtcNow;
        //             _context.Users.Update(user);
        //             await _context.SaveChangesAsync();

        //             return Ok(new { message = "User status updated." });
        //         }

        //         // GET: /api/admin/reports
        //         [HttpGet("reports")]
        //         public async Task<IActionResult> GetReports()
        //         {
        //             var reports = await _context.Complaints
        //                 .OrderByDescending(c => c.CreatedAt)
        //                 .ToListAsync();

        //             return Ok(reports);
        //         }

        //         // PUT: /api/admin/config
        //         [HttpPut("config")]
        //         public async Task<IActionResult> UpdateCommissionConfig([FromBody] Commission updatedConfig)
        //         {
        //             var config = await _context.Commissions.FirstOrDefaultAsync(c => c.Key == updatedConfig.Key);
        //             if (config == null) return NotFound("Config not found.");

        //             config.Value = updatedConfig.Value;
        //             config.Description = updatedConfig.Description;
        //             config.UpdatedAt = DateTime.UtcNow;

        //             _context.Commissions.Update(config);
        //             await _context.SaveChangesAsync();

        //             return Ok(new { message = "Config updated." });
        //         }

        //         // GET: /api/admin/analytics
        //         [HttpGet("analytics")]
        //         public async Task<IActionResult> GetAnalytics()
        //         {
        //             var latest = await _context.Analytics
        //                 .OrderByDescending(a => a.Date)
        //                 .FirstOrDefaultAsync();

        //             if (latest == null)
        //                 return NotFound("No analytics data found.");

        //             return Ok(latest);
        //         }

        //         // POST: /api/admin/notifications
        //         [HttpPost("notifications")]
        //         public async Task<IActionResult> SendNotifications([FromBody] string message)
        //         {
        //             // This is placeholder logic — depends on how notifications are stored/sent
        //             var users = await _context.Users.ToListAsync();

        //             foreach (var user in users)
        //             {
        //                 // save/send notification (e.g., save to DB, email, push, etc.)
        //                 Console.WriteLine($"Notification sent to {user.Email}: {message}");
        //             }

        //             return Ok(new { message = "Notifications sent to all users." });
        //         }
        //     }
        // }

        
