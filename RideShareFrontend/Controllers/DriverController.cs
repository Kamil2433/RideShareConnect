using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace RideShareConnect.Controllers
{

    public class DriverController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Loads Views/Driver/Index.cshtml
        }

        public IActionResult VehicleManagement()
        {
            return View(); // Loads Views/Driver/VehicleManagement.cshtml
        }

        public IActionResult DriverProfile()
        {
            return View(); // Loads Views/Driver/DriverProfile.cshtml
        }

        [HttpGet]
        public IActionResult PostRide()
        {
            return View(); // This will look for Views/Driver/PostRide.cshtml
        }

    }


}