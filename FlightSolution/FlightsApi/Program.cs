using FlightsApi.Controllers;
using FlightsBl.Interfaces;
using FlightsBl.Services;
using FlightsDl;
using FlightsDl.Interfaces;
using FlightsDl.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using FlightsEntity;
using System.Text;
using AutoMapper;
using FlightsBl;
using System.Text.Json.Serialization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddScoped<IFlightsBl, FlightsBl.Services.FlightsBl>();
builder.Services.AddScoped<IflightsDl, FlightsDl.Services.FlightsDl>();
builder.Services.AddScoped<IUserLogInBl,UserLogInBl>();
builder.Services.AddScoped<IUserLoginDl,UserLoginDl>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
var appSettingSection = builder.Configuration.GetSection("AppSetting");
builder.Services.Configure<AppSetting>(appSettingSection);
var appSetting = appSettingSection.Get<AppSetting>();
var jwt = appSetting.Jwt;

var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "flights.db");

builder.Services.AddDbContext<FlightDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt.SecretKey)),
            ClockSkew = TimeSpan.Zero


        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies[CookiesKeys.AccessToken];
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .WithOrigins("http://localhost:3000", "http://localhost:5173", "https://localhost:7157", "https://localhost:44375", "http://localhost:44375") 
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()); 
});
// options.UseSqlite(builder.Configuration.GetConnectionString("\"Data Source=flights.db\"")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Seed database with initial user
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FlightDbContext>();
    context.Database.EnsureCreated();
    
    // Check if any users exist, if not create a default user
    if (!context.UserLogIns.Any())
    {
        var defaultUser = new FlightsEntity.UserLogIn
        {
            Username = "admin",
            Password = "admin123",
            CreatedAt = DateTime.UtcNow
        };
        context.UserLogIns.Add(defaultUser);
        context.SaveChanges();
        Console.WriteLine("Default user created: admin/admin123");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Load secrets from environment variables (if present)
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
if (!string.IsNullOrEmpty(jwtSecret))
{
    appSetting.Jwt.SecretKey = jwtSecret;
}
var connStr = Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__FLIGHTS");
if (!string.IsNullOrEmpty(connStr))
{
    appSetting.ConnectionString.Flights = connStr;
}

app.Run();
