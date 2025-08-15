using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();

//Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhostClient", policy =>
    {
        policy.WithOrigins("http://localhost:5286") // Blazor client URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Enable CORS
app.UseCors("AllowLocalhostClient");


// Map API endpoints
app.MapGet("/api/productlist", (IMemoryCache cache) =>
{
    // Try to get the cached data
    if (!cache.TryGetValue("ProductList", out object cachedProducts))
    {
        // If not cached, create it
        cachedProducts = new[]
        {
            new { Id = 1, Name = "Laptop", Price = 1200.50, Stock = 25, Category = new { Id = 101, Name = "Electronics" } },
            new { Id = 2, Name = "Headphones", Price = 50.00, Stock = 100, Category = new { Id = 102, Name = "Accessories" } }
        };

        // Set cache options (e.g., expire after 5 minutes)
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        cache.Set("ProductList", cachedProducts, cacheOptions);
    }

    return cachedProducts;
});

app.Run();

