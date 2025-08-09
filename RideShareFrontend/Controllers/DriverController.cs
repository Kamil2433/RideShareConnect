using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RideShareFrontend.DTOs;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Security.Claims;
using System.Text;
using System.Net;

using RideShareConnect.Models;


namespace RideShareConnect.Controllers
{
    [Authorize(Roles = "Driver")]
    public class DriverController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DriverController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PostRide()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Msg = TempData["Msg"];
            ViewBag.Success = TempData["Success"];
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> BookingView()
        {
            var token = HttpContext.Request.Cookies["jwt"];

            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Unauthorized. Please log in.";
                Console.WriteLine("JWT cookie not found.");
                return RedirectToAction("Login", "Account"); // adjust route if needed
            }

            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add("Cookie", $"jwt={token}");

                var response = await client.GetAsync("api/ride/bookings");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var bookings = JsonSerializer.Deserialize<List<RideBookingDto>>(json);
                    return View(bookings);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                TempData["Error"] = "Failed to fetch bookings.";
                return View(new List<RideBookingDto>());
            }
            catch (HttpRequestException httpEx)
            {
                TempData["Error"] = $"Service unavailable: {httpEx.Message}";
                Console.WriteLine($"HTTP Exception: {httpEx}");
                return View(new List<RideBookingDto>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occurred.";
                Console.WriteLine($"Unexpected error: {ex}");
                return View(new List<RideBookingDto>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostRide(RideCreateDto ride)
        {
            if (!ModelState.IsValid)
            {
                TempData["Msg"] = "Model invalid";
                return View(ride);
            }

            var token = HttpContext.Request.Cookies["jwt"];
            if (string.IsNullOrEmpty(token))
            {
                TempData["Msg"] = "Unauthorized. Please log in.";
                Console.WriteLine("JWT cookie not found. in frontend");
                return RedirectToAction("PostRide");
            }

            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add("Cookie", $"jwt={token}");

                var response = await client.PostAsJsonAsync("api/Ride", ride);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Ride posted successfully!";
                    return RedirectToAction("PostRide");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Failed to post ride. Please try again.";
                Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                ModelState.AddModelError("", $"API Error: {response.StatusCode} - {errorContent}");
                return View(ride);
            }
            catch (HttpRequestException httpEx)
            {
                TempData["Error"] = $"Service unavailable: {httpEx.Message}";
                Console.WriteLine($"HTTP Request Exception: {httpEx}");
                ModelState.AddModelError("", "Service unavailable. Please try again later.");
                return View(ride);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "A system error occurred. Please contact support.";
                Console.WriteLine($"Unexpected Error: {ex}");
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
                client.DefaultRequestHeaders.UserAgent.ParseAdd("RideShareApp/1.0");

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
                client.DefaultRequestHeaders.UserAgent.ParseAdd("RideShareApp/1.0");

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