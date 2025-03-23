using BugTracker.Data;
using BugTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using BugTracker;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregar la configuración del contexto de base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.MapControllers(); // Agregar el middleware de controladores

// Configurar el middleware de la aplicación
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BugTracker API v1"));
}

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.EnsureCreatedAsync(); // Asegura que la BD existe

    if (!await context.Tickets.AnyAsync()) // Verifica si ya hay datos
    {
        context.Tickets.AddRange(new List<Ticket>
        {
            new Ticket { Title = "Error en Login", Description = "No se puede autenticar con credenciales correctas", Status = "Open", UserId = 1, User = new User { Id = 1, Name = "User1", Email = "user1@example.com" } },
            new Ticket { Title = "Pantalla en Blanco", Description = "La pantalla se queda en blanco después de iniciar sesión", Status = "Open", UserId = 2, User = new User { Id = 2, Name = "User2", Email = "user2@example.com" } },
            new Ticket { Title = "Carga Lenta", Description = "La aplicación tarda demasiado en responder", Status = "Closed", UserId = 3, User = new User { Id = 3, Name = "User3", Email = "user3@example.com" } }
        });

        await context.SaveChangesAsync();
    }
}

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
.WithName("GetWeatherForecast");

await app.RunAsync();

namespace BugTracker
{
    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}