using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Net;

using RideShareConnect.Models;

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
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5157/";
            _httpClient.BaseAddress = new Uri(apiBaseUrl);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            // Ensure cookies are sent with requests
            _httpClient.DefaultRequestHeaders.ConnectionClose = false; // Keep connection alive for cookie handling
        }

        [HttpGet]
        public IActionResult Index()
        {
            var token = HttpContext.Request.Cookies["jwt"];
            _logger.LogInformation("Authenticated: {IsAuthenticated}, Role: {Role}, JWT: {Token}", 
                User.Identity.IsAuthenticated, User.FindFirst(ClaimTypes.Role)?.Value, token);
            return View();
        }

        

 [HttpGet]
public async Task<IActionResult> PassengerProfile()
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
public async Task<IActionResult> PassengerProfile(UserProfileViewModel model)
{
    if (!ModelState.IsValid)
    {
        _logger.LogWarning("ModelState invalid: {Errors}",
            string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

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
        var checkRequest = new HttpRequestMessage(HttpMethod.Get, "api/UserProfile/me");
        checkRequest.Headers.Add("Accept", "application/json");
        checkRequest.Headers.Add("Cookie", $"jwt={jwtCookie}");

        var checkResponse = await _httpClient.SendAsync(checkRequest);
        var isNewProfile = checkResponse.StatusCode == HttpStatusCode.NotFound;

        // 🔁 Step 2: Prepare content
        var json = JsonSerializer.Serialize(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // 🔁 Step 3: Call appropriate API
        var endpoint = isNewProfile ? "api/UserProfile" : "api/UserProfile/me";
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

            return RedirectToAction("PassengerProfile");
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
public async Task<IActionResult> DeleteProfile()
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