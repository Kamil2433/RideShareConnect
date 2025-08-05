using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using RideShareConnect.Models;
using System.Text;
using System.Text.Json;

namespace RideShareConnect.Controllers
{
    [Authorize(Roles = "Passenger")]
    public class PassengerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PassengerController> _logger;

        public PassengerController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<PassengerController> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _logger = logger;
            
            // Set base address for API
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"];
            _httpClient.BaseAddress = new Uri(apiBaseUrl);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        // Set JWT token from cookies to Authorization header
        private void SetAuthorizationHeader()
        {
            var token = HttpContext.Request.Cookies["jwt"];
            Console.WriteLine(token,"This is token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                _logger.LogInformation("JWT token set in Authorization header");
            }
            else
            {
                _logger.LogWarning("No JWT token found in cookies");
            }
        }

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
            return View();
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

         public IActionResult SearchRide()
        {
            return View();
        }

        [HttpGet("pgeocode")]
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


        [HttpGet("preverse-geocode")]
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

        [HttpGet]
        public async Task<IActionResult> PassengerProfile()
        {
            try
            {
                SetAuthorizationHeader();
                
                // Call API to get existing profile
                var response = await _httpClient.GetAsync("api/UserProfile/me");
                
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var profile = JsonSerializer.Deserialize<UserProfileViewModel>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    if (profile != null)
                    {
                        profile.IsNewProfile = false;
                        _logger.LogInformation("Profile loaded successfully");
                        return View(profile);
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // No profile exists, show create form
                    _logger.LogInformation("No profile found, showing create form");
                    return View(new UserProfileViewModel { IsNewProfile = true });
                }
                else
                {
                    _logger.LogError($"API call failed with status: {response.StatusCode}");
                    TempData["ErrorMessage"] = "Error loading profile. Please try again.";
                    return View(new UserProfileViewModel { IsNewProfile = true });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading passenger profile");
                TempData["ErrorMessage"] = "Error loading profile. Please try again.";
            }
            
            return View(new UserProfileViewModel { IsNewProfile = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PassengerProfile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                SetAuthorizationHeader();
                
                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                HttpResponseMessage response;
                string successMessage;

                if (model.IsNewProfile)
                {
                    // Create new profile - POST to api/UserProfile
                    response = await _httpClient.PostAsync("api/UserProfile", content);
                    successMessage = "Profile created successfully!";
                    _logger.LogInformation("Attempting to create new profile");
                }
                else
                {
                    // Update existing profile - POST to api/UserProfile/me
                    response = await _httpClient.PostAsync("api/UserProfile/me", content);
                    successMessage = "Profile updated successfully!";
                    _logger.LogInformation("Attempting to update existing profile");
                }

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = successMessage;
                    model.IsNewProfile = false; // After creation, it's no longer new
                    _logger.LogInformation("Profile saved successfully");
                    return View(model);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"API call failed with status: {response.StatusCode}, Error: {errorContent}");
                    TempData["ErrorMessage"] = "Failed to save profile. Please try again.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving passenger profile");
                TempData["ErrorMessage"] = "An error occurred while saving the profile.";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProfile()
        {
            try
            {
                SetAuthorizationHeader();
                
                var response = await _httpClient.DeleteAsync("api/UserProfile/me");
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Profile deleted successfully!";
                    _logger.LogInformation("Profile deleted successfully");
                    return RedirectToAction("PassengerProfile");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Delete API call failed with status: {response.StatusCode}, Error: {errorContent}");
                    TempData["ErrorMessage"] = "Failed to delete profile.";
                    return RedirectToAction("PassengerProfile");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting passenger profile");
                TempData["ErrorMessage"] = "An error occurred while deleting the profile.";
                return RedirectToAction("PassengerProfile");
            }
        }

        // For AJAX calls if needed
        [HttpGet]
        public async Task<IActionResult> GetProfileData()
        {
            try
            {
                SetAuthorizationHeader();
                
                var response = await _httpClient.GetAsync("api/UserProfile/me");
                
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var profile = JsonSerializer.Deserialize<UserProfileViewModel>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    return Json(new { success = true, data = profile });
                }
                else
                {
                    return Json(new { success = false, message = "Profile not found" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting profile data");
                return Json(new { success = false, message = "Error loading profile data" });
            }
        }

        // For handling profile picture upload
        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    // Convert to base64
                    using var memoryStream = new MemoryStream();
                    await file.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    var base64String = Convert.ToBase64String(fileBytes);
                    var dataUrl = $"data:{file.ContentType};base64,{base64String}";

                    return Json(new { success = true, imageData = dataUrl });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error uploading profile picture");
                    return Json(new { success = false, message = "Error uploading image" });
                }
            }

            return Json(new { success = false, message = "No file selected" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _httpClient?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}