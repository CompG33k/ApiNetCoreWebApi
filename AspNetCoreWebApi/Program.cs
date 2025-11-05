using AspNetCoreWebApi.BusinessLayer;
using AspNetCoreWebApi.BusinessLayer.Interfaces;
using AspNetCoreWebApi.Clients;
using AspNetCoreWebApi.Clients.Interfaces;

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
