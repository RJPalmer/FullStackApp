var builder = WebApplication.CreateBuilder(args);
// 1️⃣ Configure CORS policy
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

app.UseCors("AllowLocalhostClient");

app.MapGet("/api/productlist", () =>

{

    return new[]

    {

        new { Id = 1, Name = "Laptop", Price = 1200.50, Stock = 25,  Category = new { Id = 101, Name = "Electronics" } },

        new { Id = 2, Name = "Headphones", Price = 50.00, Stock = 100, Category = new { Id = 102, Name = "Accessories" } }

    };

});

app.Run();

