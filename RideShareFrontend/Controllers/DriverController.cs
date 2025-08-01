using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace RideShareConnect.Controllers
{
[Authorize(Roles = "Driver")]
    public class DriverController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Loads Views/Driver/Index.cshtml
        }


        [HttpGet]
        public IActionResult PostRide()
        {
            return View(); // This will look for Views/Driver/PostRide.cshtml
        }

    }


}