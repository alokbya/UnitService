using System.Text.Json;

namespace UnitService.TestClient;

public class Program
{
    private static readonly HttpClient client = new HttpClient();
    private const string BaseUrl = "http://localhost:5278/api/v1/units";

    public static async Task Main()
    {
        Console.WriteLine("Temperature Conversion Service Test Client\n");

        // Test single conversion
        await TestSingleConversion(32, "f", "c");

        // Test bulk conversion
        await TestBulkConversion();

        // Test unit information
        await TestUnitInfo();
    }

    private static async Task TestSingleConversion(double value, string fromUnit, string toUnit)
    {
        Console.WriteLine($"Testing single conversion: {value}{fromUnit} to {toUnit}");
        
        try
        {
            var response = await client.GetAsync(
                $"{BaseUrl}/convert?value={value}&fromUnit={fromUnit}&toUnit={toUnit}");
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Result: {result}\n");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}\n");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}\n");
        }
    }

    private static async Task TestBulkConversion()
    {
        Console.WriteLine("Testing bulk conversion");
        
        var request = new
        {
            conversions = new[]
            {
                new { value = 0, fromUnit = "C", toUnit = "F" },
                new { value = 100, fromUnit = "C", toUnit = "F" }
            }
        };

        try
        {
            var response = await client.PostAsync(
                $"{BaseUrl}/convert/bulk",
                new StringContent(
                    JsonSerializer.Serialize(request),
                    System.Text.Encoding.UTF8,
                    "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Result: {result}\n");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}\n");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}\n");
        }
    }

    private static async Task TestUnitInfo()
    {
        Console.WriteLine("Testing unit information endpoint");
        
        try
        {
            var response = await client.GetAsync($"{BaseUrl}/info");
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Result: {result}\n");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}\n");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}\n");
        }
    }
}