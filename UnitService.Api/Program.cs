using System.Threading.RateLimiting;
using UnitService.Core.Interfaces;
using UnitService.Core.Services;
using UnitService.Core.Models;
using UnitService.Contracts.Requests;
using UnitService.Contracts.Responses;
using UnitService.Core.Extensions;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Host.UseSerilog((context, config) => 
    config.WriteTo.Console());

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Temperature Conversion API",
        Description = "Supports single-letter units (c, f, k) and full names (celsius, fahrenheit, kelvin)",
        Version = "v1"
    });
});

// Add rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
});

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IUnitConverter, TemperatureConverter>();

// Add health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRateLimiter();

// Add health check endpoint
app.MapHealthChecks("/health");

// Define endpoints
var api = app.MapGroup("/api/v1/units")
    .WithOpenApi()
    .WithTags("Unit Conversion");

// Single conversion
api.MapGet("/convert", async (
    double value,
    string fromUnit,
    string toUnit,
    IUnitConverter converter) =>
{
    try
    {
        var sourceUnit = TemperatureUnitExtensions.ParseUnit(fromUnit);
        var targetUnit = TemperatureUnitExtensions.ParseUnit(toUnit);
        
        var source = new Temperature(value, sourceUnit);
        var result = converter.Convert(source, targetUnit);
        
        return Results.Ok(new ConversionResponse(
            source.Value,
            source.Unit.ToString(),
            result.Value,
            result.Unit.ToString()
        ));
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

// Update bulk conversion endpoint similarly
api.MapPost("/convert/bulk", async (
    BulkConversionRequest request,
    IUnitConverter converter) =>
{
    try
    {
        var results = request.Conversions.Select(c =>
        {
            var sourceUnit = TemperatureUnitExtensions.ParseUnit(c.FromUnit);
            var targetUnit = TemperatureUnitExtensions.ParseUnit(c.ToUnit);
            
            var source = new Temperature(c.Value, sourceUnit);
            var result = converter.Convert(source, targetUnit);
            
            return new ConversionResponse(
                source.Value,
                source.Unit.ToString(),
                result.Value,
                result.Unit.ToString()
            );
        });
        
        return Results.Ok(results);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

// Get supported units
api.MapGet("/info", async (IUnitConverter converter) =>
{
    var units = converter.GetSupportedUnits();
    var response = units.Select(u => new UnitInfoResponse(
        u.Key.ToString(),
        u.Value.Minimum,
        u.Value.Maximum
    ));
    
    return Results.Ok(response);
})
.WithName("GetUnitInfo")
.WithOpenApi();

app.Use(async (context, next) =>
{
    var start = DateTime.UtcNow;
    
    await next();
    
    var elapsed = DateTime.UtcNow - start;
    Log.Information(
        "Request {Method} {Path} completed in {Elapsed}ms with status {StatusCode}",
        context.Request.Method,
        context.Request.Path,
        elapsed.TotalMilliseconds,
        context.Response.StatusCode);
});

app.Run();