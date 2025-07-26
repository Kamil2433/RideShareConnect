using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

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

var app = builder.Build();

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

// Route config
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
