using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using RideShareConnect.Models;
using System.Text;
using System.Text.Json;

namespace RideShareConnect.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AdminController : Controller
    {

        public IActionResult Index()
        {
            Console.WriteLine("Authenticated: " + User.Identity.IsAuthenticated);
            Console.WriteLine("Role: " + User.FindFirst(ClaimTypes.Role)?.Value);
            var token = HttpContext.Request.Cookies["jwt"];
            Console.WriteLine("🔴 RAW COOKIE JWT: " + token);
            if (string.IsNullOrEmpty(token))
                Console.WriteLine("❌ No JWT in Cookie");
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            foreach (var claim in jwt.Claims)
            {
                Console.WriteLine($"🔹 Claim: {claim.Type} = {claim.Value}");
            }
            return View();
        }

        public IActionResult Analytics()
        {
            return View();
        }
        public IActionResult Complaints()
        {
            return View();
        }
        public IActionResult PaymentStatus()
        {
            return View();
        }
        public IActionResult Verification()
        {
            return View();
        }
        public IActionResult LoginHistory()
        {
            return View();
        }
        

    }
}
