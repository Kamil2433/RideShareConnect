using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;


namespace RideShareConnect.Controllers
{
     [Authorize(Roles = "Passenger")]
    public class PassengerController : Controller
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
     public IActionResult Wallet()
    {
        return View(); // Will look for Views/Passenger/Wallet.cshtml
    }

    public IActionResult History()
    {
        return View();
    }
    public IActionResult Payments()
    {
        return View();
    }
     public IActionResult AcceptRide()
    {
        return View();
    }
    
    }

    
}
