using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RideShareConnect.Controllers
{
    
    public class PassengerController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Loads Views/Passenger/Index.cshtml
        }
     public IActionResult Wallet()
    {
        return View(); // Will look for Views/Passenger/Wallet.cshtml
    }

    public IActionResult History()
    {
        return View();
    }
    
    }

    
}
