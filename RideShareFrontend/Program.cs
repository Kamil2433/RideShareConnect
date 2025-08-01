using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
// ✅ Add session support
builder.Services.AddSession();

// ✅ Register session-backed TempData provider
//builder.Services.AddSingleton<ITempDataProvider, SessionStateTempDataProvider>();
builder.Services.AddSingleton<ITempDataProvider, SessionStateTempDataProvider>();

// If you're using cookie auth only for temporary manual role-based view control
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Index"; // still needed if cookie-based login used manually
        options.AccessDeniedPath = "/Home/AccessDenied";

        // Prevent redirect to login when unauthorized — let frontend handle it
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
    });

// Define role-based access policies (optional if not used on controller level)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PassengerOnly", policy => policy.RequireRole("Passenger"));
    options.AddPolicy("DriverOnly", policy => policy.RequireRole("Driver"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddControllersWithViews();

// ✅ Register IHttpClientFactory
builder.Services.AddHttpClient();

// ✅ Optional but recommended if you're using TempData/Session
builder.Services.AddSession();


var app = builder.Build();

app.UseCors("AllowAll");
// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // This is fine, even if auth is via frontend
app.UseAuthorization();
app.UseSession();


// Route config
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
