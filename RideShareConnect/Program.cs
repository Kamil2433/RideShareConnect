using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RideShareConnect.Data;
using RideShareConnect.Repository.Interfaces;
using RideShareConnect.Repository.Implements;
using RideShareConnect.Services;
using RideShareConnect.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Configure EF Core with MSSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(Module1AutoMapperProfile));
builder.Services.AddAutoMapper(typeof(UserProfileAutoMapperProfile).Assembly);


// Register repositories and services
builder.Services.AddScoped<IUserAuthRepository, UserAuthRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5125")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMaintenanceRecordRepository, MaintenanceRecordService>();
builder.Services.AddScoped<IVehicleDocumentServiceRepository, VehicleDocumentService>();
builder.Services.AddScoped<IDriverProfileServiceRepository, DriverProfileService>(); 
builder.Services.AddScoped<IVehicleRepository, VehicleService>();                     
builder.Services.AddScoped<IDriverRatingRepository, DriverRatingService>();  
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.Cookie.Name = "jwt";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;

        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = 403;
            return Task.CompletedTask;
        };
    });
  builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCors("AllowFrontend");
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax, // Allow cross-origin redirects
    Secure = CookieSecurePolicy.None // For local HTTP
});
app.Use(async (context, next) =>
{
     var jwt = context.Request.Cookies["jwt"];
    Console.WriteLine("🔍 Backend sees JWT cookie: " + jwt);
    await next();
});
app.Use(async (context, next) =>
{
    var jwt = context.Request.Cookies["jwt"];
    Console.WriteLine("🔍 Backend sees JWT cookie: " + jwt);

    if (!string.IsNullOrEmpty(jwt))
    {
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        try
        {
            var token = handler.ReadJwtToken(jwt);
            var identity = new System.Security.Claims.ClaimsIdentity(token.Claims, "Cookies");
            var principal = new System.Security.Claims.ClaimsPrincipal(identity);
            context.User = principal;

            Console.WriteLine("✅ Backend set HttpContext.User from cookie.");
            Console.WriteLine("👤 Identity.IsAuthenticated: " + context.User.Identity?.IsAuthenticated);
            Console.WriteLine("🔑 Role: " + context.User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value);
        }
        catch (Exception ex)
        {
           Console.WriteLine("❌ Failed to parse JWT: " + ex.Message);
        }
    }

    await next();
});

app.UseAuthentication(); 
app.Use(async (context, next) =>
{
    Console.WriteLine("👤 Identity.IsAuthenticated: " + context.User.Identity?.IsAuthenticated);
    Console.WriteLine("🔑 Role: " + context.User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value);
    await next();
});

app.UseAuthorization();
app.MapControllers();

app.Run();