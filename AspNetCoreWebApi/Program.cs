using AspNetCoreWebApi.BusinessLayer;
using AspNetCoreWebApi.BusinessLayer.Interfaces;
using AspNetCoreWebApi.Clients;
using AspNetCoreWebApi.Clients.Interfaces;
using Newtonsoft.Json;
using NuGet.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ISummaries,Summaries>();
builder.Services.AddHttpClient();
builder.Services.AddTransient<IApiClient, WeatherApiClient>();
builder.Logging.AddLog4Net();

var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.WriteIndented = true; // For development/pretty printing
    options.SerializerOptions.PropertyNameCaseInsensitive = true; // Common requirement
    options.SerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Example: Serialize enums as strings
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add these lines

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
