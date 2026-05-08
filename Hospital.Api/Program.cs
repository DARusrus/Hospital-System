using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Hospital.Core.Interfaces.Repositories;
using Hospital.Infrastructure.Repositories;
using Hospital.Api.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hospital.Core.Validators.Clinic;
using Hospital.Core.Interfaces.Services;
using Hospital.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------
// Choose connection string depending on runtime
// ---------------------------------------------
var connectionString = builder.Environment.IsDevelopment()
    ? builder.Configuration.GetConnectionString("EfLocalConnection")  // Used when running EF Core from host
    : builder.Configuration.GetConnectionString("DefaultConnection"); // Used when API runs inside Docker

// ---------------------------------------------
// Add controllers + FluentValidation (correct modern config)
// ---------------------------------------------
builder.Services.AddControllers();

builder.Services
    .AddFluentValidationAutoValidation()                  // enables automatic validation
    .AddFluentValidationClientsideAdapters();             // optional but recommended for UI

// Automatically register validators from Hospital.Core
builder.Services.AddValidatorsFromAssemblyContaining<ClinicCreateValidator>();

// ---------------------------------------------
// Register DbContext
// ---------------------------------------------
builder.Services.AddDbContext<HospitalDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// ---------------------------------------------
// Repository registration
// ---------------------------------------------
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IClinicRepository, ClinicRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

// ---------------------------------------------
// Service registration (THIS WAS MISSING before!)
// ---------------------------------------------
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IClinicService, ClinicService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

// ---------------------------------------------
// Swagger
// ---------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ---------------------------------------------
// Swagger only in Development environments
// ---------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global exception handler (must run early to catch downstream exceptions)
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

// ---------------------------------------------
// Default template endpoint (safe to keep)
// ---------------------------------------------
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

// ---------------------------------------------
// Map controllers for real API endpoints
// ---------------------------------------------
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

