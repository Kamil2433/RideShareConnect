using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RideShareFrontend.DTOs;
using System.Net.Http.Headers;

namespace RideShareConnect.Controllers
{
[Authorize(Roles = "Driver")]
    public class DriverController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public DriverController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View(); // Loads Views/Driver/Index.cshtml
        }

        [HttpGet]
        public IActionResult PostRide()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Msg = TempData["Msg"];
            ViewBag.Success = TempData["Success"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostRide(RideCreateDto ride)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Msg"] = "Model invalid";
                    Console.WriteLine("err");
                    
                    return View(ride);
                }

                 var token = context.Request.Cookies["jwt"];

                Console.WriteLine(token);
                if (string.IsNullOrEmpty(token))
                {
                    TempData["Msg"] = "Unauthorized. Please log in.";
                    Console.WriteLine("unauth");
                return RedirectToAction("PostRide"); // Important: must be a redirect for TempData to work as expected
                }

                try
                {
                    var client = _httpClientFactory.CreateClient();
                    client.BaseAddress = new Uri(_configuration["ApiBaseUrl"]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    Console.WriteLine("Attempting to post ride to API");

                    var response = await client.PostAsJsonAsync("api/ride", ride);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Success"] = "Ride posted successfully!";
                        return RedirectToAction("PostRide");
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");

                        TempData["Error"] = "Failed to post ride. Please try again.";
                        ModelState.AddModelError("", $"API Error: {response.StatusCode} - {errorContent}");
                        return View(ride);
                    }
                }
                catch (HttpRequestException httpEx)
                {
                    Console.WriteLine($"HTTP Request Exception: {httpEx.Message}");
                    TempData["Error"] = "Service unavailable. Please try again later.";
                    ModelState.AddModelError("", "Service unavailable. Please try again later.");
                    return View(ride);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected Error: {ex.Message}");
                    TempData["Error"] = "An unexpected error occurred. Please try again.";
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    return View(ride);
                }
            }
            catch (Exception globalEx)
            {
                Console.WriteLine($"Global Exception: {globalEx.Message}");
                TempData["Error"] = "A system error occurred. Please contact support.";
                return RedirectToAction("PostRide");
            }
        }


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


        [HttpGet("reverse-geocode")]
        [AllowAnonymous]
        public async Task<IActionResult> ReverseGeocode([FromQuery] double lat, [FromQuery] double lng)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("RideShareApp/1.0 (kamilmulani2433@gmail.com)");

                var url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={lat}&lon={lng}";
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return Content(content, "application/json");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(503, $"Reverse geocoding service unavailable: {ex.Message}");
            }
        }

    }
}
