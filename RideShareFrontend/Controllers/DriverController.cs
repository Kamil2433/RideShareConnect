using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RideShareFrontend.DTOs;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Security.Claims;
using System.Text;
using System.Net;
using RideShareConnect.Models;

using RideShareFrontend.Models.DTOs;

namespace RideShareFrontend.Controllers
{
    [Authorize(Roles = "Driver")]
    public class DriverController : Controller
    {

        private readonly HttpClient _httpClient;


        //iconfiguration and logger for dependency injection, this is used to fetch the base URL for the API and log messages form appsetting.json file
        private readonly IConfiguration _configuration;
        private readonly ILogger<DriverController> _logger;

        public DriverController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _logger = logger;

            // Set base address for API
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5157/";
            _httpClient.BaseAddress = new Uri(apiBaseUrl);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            // Ensure cookies are sent with requests
            _httpClient.DefaultRequestHeaders.ConnectionClose = false; // Keep connection alive for cookie handling
        }



        public IActionResult Index()
        {
            // return View();
    return RedirectToAction("PostRide", "Driver");
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
        public async Task<IActionResult> VehicleManagement()
        {
            var model = new VehicleRegistrationViewModel();

            try
            {
                var jwtCookie = HttpContext.Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwtCookie))
                {
                    TempData["ErrorMessage"] = "Unauthorized: JWT missing.";
                    return RedirectToAction("Index");
                }

                // Request to get existing vehicles for logged-in driver
                var request = new HttpRequestMessage(HttpMethod.Get, "/api/vehicle/my-vehicles");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Cookie", $"jwt={jwtCookie}");

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    // Deserialize the JSON response to a list of VehicleRegistrationViewModel
                    var vehicles = JsonSerializer.Deserialize<List<VehicleRegistrationViewModel>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
         
                    if (vehicles != null && vehicles.Count > 0)
                    {
                        var firstVehicle = vehicles[0];
                        model.VehicleType = firstVehicle.VehicleType;
                        model.InsuranceNumber = firstVehicle.InsuranceNumber;
                        model.RegistrationExpiry = firstVehicle.RegistrationExpiry;
                        model.RCDocumentBase64 = firstVehicle.RCDocumentBase64;
                        model.InsuranceDocumentBase64 = firstVehicle.InsuranceDocumentBase64;
                        model.LicensePlate = firstVehicle.LicensePlate;

                    }
                }
                else
                {
                    _logger.LogWarning("Failed to load existing vehicles: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                // Log the exception and show an error message
                _logger.LogError(ex, "Exception while loading existing vehicles for VehicleManagement.");
            }

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VehicleManagement(VehicleRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid Vehicle Registration model: {Errors}",
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return View(model);
            }

            try
            {
                var jwtCookie = HttpContext.Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwtCookie))
                {
                    TempData["ErrorMessage"] = "Unauthorized: JWT missing.";
                    return RedirectToAction("Index");
                }



                var dto = new
                {
                    model.VehicleType,
                    model.LicensePlate,
                    model.InsuranceNumber,
                    model.RegistrationExpiry,
                    model.RCDocumentBase64,
                    model.InsuranceDocumentBase64,
                };

                var json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, "/api/vehicle/register")
                {
                    Content = content
                };
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Cookie", $"jwt={jwtCookie}");

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Vehicle registered successfully!";
                    return RedirectToAction("VehicleManagement");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Vehicle registration API failed: {StatusCode} - {Error}", response.StatusCode, errorContent);
                    TempData["ErrorMessage"] = "Failed to register vehicle.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during vehicle registration.");
                TempData["ErrorMessage"] = "An error occurred while registering the vehicle.";
                return View(model);
            }
        }



     
 [HttpGet]
public async Task<IActionResult> DriverProfile()
{
    try
    {
        var jwtCookie = HttpContext.Request.Cookies["jwt"];
        if (string.IsNullOrEmpty(jwtCookie))
        {
            _logger.LogWarning("JWT cookie not found in request");
            TempData["ErrorMessage"] = "You are not logged in.";
            return RedirectToAction("Index");
        }
        var request = new HttpRequestMessage(HttpMethod.Get, "api/UserProfile/me");
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Cookie", $"jwt={jwtCookie}");
        var response = await _httpClient.SendAsync(request);
        _logger.LogInformation("Response Status: {StatusCode}", response.StatusCode);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var profile = JsonSerializer.Deserialize<UserProfileViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (profile != null)
            {
                TempData["Error"] = $"Service unavailable: {httpEx.Message}";
                return View(new List<RideDto>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Unexpected error: {ex.Message}";
                return View(new List<RideDto>());
            }
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

        [HttpPost("approve-booking")]
        public async Task<IActionResult> ApproveBooking(int bookingId, bool isApproved)
        {
            var token = HttpContext.Request.Cookies["jwt"];
            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Unauthorized. Please log in.";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add("Cookie", $"jwt={token}");

                var response = await client.PostAsJsonAsync("api/ride/booking/approve", new
                {
                    bookingId = bookingId,
                    isApproved = isApproved
                });

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = $"Booking {(isApproved ? "approved" : "rejected")} successfully!";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["Error"] = $"Failed to update booking status: {errorContent}";
                }
            }
            catch (HttpRequestException httpEx)
            {
                TempData["Error"] = $"Service unavailable: {httpEx.Message}";
                Console.WriteLine($"HTTP Exception: {httpEx}");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occurred.";
                Console.WriteLine($"Unexpected error: {ex}");
            }

            return RedirectToAction("BookingView");
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

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Profile deleted successfully!";
                    _logger.LogInformation("Profile deleted successfully");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Delete API call failed with status: {StatusCode}, Error: {Error}", response.StatusCode, errorContent);
                    TempData["ErrorMessage"] = "Failed to delete profile.";
                }

                return RedirectToAction("PassengerProfile");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting passenger profile");
                TempData["ErrorMessage"] = "An error occurred while deleting the profile.";
                return RedirectToAction("PassengerProfile");
            }
        }

        protected override void Dispose(bool disposing)
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