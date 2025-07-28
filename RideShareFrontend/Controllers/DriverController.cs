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


        [HttpGet]
        public IActionResult PostRide()
        {
            return View(); // This will look for Views/Driver/PostRide.cshtml
        }

        // In your Controller
        [HttpGet("geocode")]
        [AllowAnonymous]
        public async Task<IActionResult> Geocode([FromQuery(Name = "q")] string query)
        {

            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query parameter is required.");

            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("RideShareApp/1.0 (kamilmulani2433@gmail.com)");

                var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(query)}&format=json";
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return Content(content, "application/json");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(503, $"Geocoding service unavailable: {ex.Message}");
            }
        }


    }


}