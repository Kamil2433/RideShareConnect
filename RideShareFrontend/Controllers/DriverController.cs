
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Net;
using RideShareConnect.Models;

using RideShareFrontend.Models.DTOs;

namespace RideShareFrontend.Controllers
{
    [Authorize(Roles = "Driver")]
    public class DriverController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DriverController> _logger;

        public DriverController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<DriverController> logger)
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
            return View(); // Loads Views/Driver/Index.cshtml
        }

        [HttpGet]
        public IActionResult PostRide()
        {
            return View(); // This will look for Views/Driver/PostRide.cshtml
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
        public async Task<IActionResult> UserProfile()
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

                var request = new HttpRequestMessage(HttpMethod.Get, "/api/driver-profile/getdrverprofile");
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
                        profile.IsNewProfile = false;
                        return View(profile);
                    }
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return View(new UserProfileViewModel { IsNewProfile = true });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API Error - Status: {StatusCode}, Content: {Content}", response.StatusCode, errorContent);
                    TempData["ErrorMessage"] = "Failed to load profile.";
                }

                return View(new UserProfileViewModel { IsNewProfile = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while loading profile.");
                TempData["ErrorMessage"] = "An error occurred.";
                return View(new UserProfileViewModel { IsNewProfile = true });
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserProfile(UserProfileViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var jwtCookie = HttpContext.Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwtCookie))
                {
                    _logger.LogWarning("JWT cookie not found in request");
                    TempData["ErrorMessage"] = "You are not logged in.";
                    return RedirectToAction("Index");
                }

                // First get the existing driver profile to preserve driver-specific fields
                var getRequest = new HttpRequestMessage(HttpMethod.Get, "/api/driver-profile/getdrverprofile");
                getRequest.Headers.Add("Accept", "application/json");
                getRequest.Headers.Add("Cookie", $"jwt={jwtCookie}");

                var getResponse = await _httpClient.SendAsync(getRequest);

                DriverProfileDto existingProfile = null;
                if (getResponse.IsSuccessStatusCode)
                {
                    var json = await getResponse.Content.ReadAsStringAsync();
                    existingProfile = JsonSerializer.Deserialize<DriverProfileDto>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }

                // Prepare the complete request data
                var requestData = new
                {
                    model.FirstName,
                    model.LastName,
                    model.PhoneNumber,
                    model.Address,
                    model.ProfilePicture,
                    model.IsNewProfile,
                    // Use existing values for driver-specific fields if available
                    LicenseNumber = existingProfile?.LicenseNumber ?? "",
                    LicenseExpiryDate = existingProfile?.LicenseExpiryDate ?? DateTime.UtcNow.AddYears(1),
                    LicenseImageUrl = existingProfile?.LicenseImageUrl ?? "",
                    YearsOfExperience = existingProfile?.YearsOfExperience ?? 0,
                    EmergencyContactName = existingProfile?.EmergencyContactName ?? "",
                    EmergencyContactPhone = existingProfile?.EmergencyContactPhone ?? ""
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "/api/driver-profile/create-or-update")
                {
                    Content = new StringContent(
                        JsonSerializer.Serialize(requestData),
                        Encoding.UTF8,
                        "application/json")
                };

                request.Headers.Add("Cookie", $"jwt={jwtCookie}");

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Profile updated successfully!";
                    return RedirectToAction("UserProfile");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("API Error - Status: {StatusCode}, Content: {Content}", response.StatusCode, errorContent);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    TempData["ErrorMessage"] = "Session expired. Please login again.";
                    return RedirectToAction("Index");
                }

                TempData["ErrorMessage"] = "Failed to update profile. Please try again.";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while updating profile.");
                TempData["ErrorMessage"] = "An error occurred while updating your profile.";
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

                var request = new HttpRequestMessage(HttpMethod.Get, "/api/driver-profile/getdrverprofile");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Cookie", $"jwt={jwtCookie}");

                    
                var response = await _httpClient.SendAsync(request);
                _logger.LogInformation("Response Status: {StatusCode}", response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("Response JSON: " + json);
                    var profile = JsonSerializer.Deserialize<DriverProfileDto>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (profile != null)
                    {
                        profile.IsNewProfile = false;
                    ViewData["IsVerified"] = profile.isverfied;

                        return View(profile);
                    }
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return View(new DriverProfileDto { IsNewProfile = true });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API Error - Status: {StatusCode}, Content: {Content}", response.StatusCode, errorContent);
                    TempData["ErrorMessage"] = "Failed to load profile.";
                }

                return View(new DriverProfileDto { IsNewProfile = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while loading profile.");
                TempData["ErrorMessage"] = "An error occurred.";
                return View(new DriverProfileDto { IsNewProfile = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DriverProfile(DriverProfileDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid: {Errors}",
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                Console.WriteLine("ModelState is invalid");
                return View(model);
            }

            try
            {
                var jwtCookie = HttpContext.Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwtCookie))
                {
                    TempData["ErrorMessage"] = "Unauthorized: JWT missing.";
                    return RedirectToAction("PassengerProfile");
                }

                // 🔍 Step 1: Check if profile exists
                var checkRequest = new HttpRequestMessage(HttpMethod.Get, "/api/driver-profile/create-or-update");
                checkRequest.Headers.Add("Accept", "application/json");
                checkRequest.Headers.Add("Cookie", $"jwt={jwtCookie}");

                var checkResponse = await _httpClient.SendAsync(checkRequest);
                var isNewProfile = checkResponse.StatusCode == HttpStatusCode.NotFound;

                // 🔁 Step 2: Prepare content
                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // 🔁 Step 3: Call appropriate API
                var endpoint = isNewProfile ? "api/UserProfile" : "/api/driver-profile/create-or-update";
                var saveRequest = new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = content
                };
                saveRequest.Headers.Add("Accept", "application/json");
                saveRequest.Headers.Add("Cookie", $"jwt={jwtCookie}");

                var saveResponse = await _httpClient.SendAsync(saveRequest);

                if (saveResponse.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = isNewProfile
                        ? "Profile created successfully!"
                        : "Profile updated successfully!";

                    return RedirectToAction("DriverProfile");
                }
                else
                {
                    var errorContent = await saveResponse.Content.ReadAsStringAsync();
                    _logger.LogError("API save failed: {StatusCode} - {Error}", saveResponse.StatusCode, errorContent);
                    TempData["ErrorMessage"] = "Failed to save profile.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while saving profile");
                TempData["ErrorMessage"] = "An error occurred while saving the profile.";
                return View(model);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProfileP()
        {
            try
            {
                var jwtCookie = HttpContext.Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwtCookie))
                {
                    TempData["ErrorMessage"] = "Unauthorized: JWT missing.";
                    return RedirectToAction("PassengerProfile");
                }

                var request = new HttpRequestMessage(HttpMethod.Delete, "api/UserProfile/me");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Cookie", $"jwt={jwtCookie}");

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
            if (disposing)
            {
                _httpClient?.Dispose();
            }
            base.Dispose(disposing);
        }






















    }


}