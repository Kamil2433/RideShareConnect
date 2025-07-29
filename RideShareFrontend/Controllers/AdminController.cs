using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RideShareConnect.Controllers
{
    
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Loads Views/Driver/Index.cshtml
        }
    }
}
