using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using NLog.Web;
using NetProject.Data;
using NetProject.Mappers;
using NetProject.Models;

var builder = WebApplication.CreateBuilder(args);

// ────────────────────────────────────────────────────────────────────────────────
// 1) Konfiguracja NLog jako dostawcy logów
builder.Logging.ClearProviders();
builder.Host.UseNLog();
// ────────────────────────────────────────────────────────────────────────────────

// 2) Rejestracja DbContext (SQL Server)
builder.Services.AddDbContext<MyAppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3) Rejestracja Identity (użytkownicy + role)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MyAppDbContext>()
    .AddDefaultTokenProviders();

// 4) (opcjonalnie) Rejestracja Mapperly – metody są statyczne, więc nie trzeba
// builder.Services.AddMapperly();

// 5) Rejestracja autoryzacji
builder.Services.AddAuthorization();

// 6) Rejestracja Swaggera (OpenAPI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetProject API", Version = "v1" });
});

var app = builder.Build();

// ────────────────────────────────────────────────────────────────────────────────
// 7) Seed ról, konta admina i przykładowych danych
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedRolesAndAdminAsync(services);
    var dbContext = services.GetRequiredService<MyAppDbContext>();
    await DbInitializer.SeedSampleDataAsync(dbContext);
}
// ────────────────────────────────────────────────────────────────────────────────

// 8) Root endpoint – status połączenia z bazą i link do Swaggera
//    * MapGet musi być przed UseSwaggerUI(), żeby nie nadpisywał "/"
app.MapGet("/", async (MyAppDbContext db, ILogger<Program> logger) =>
{
    try
    {
        var canConnect = await db.Database.CanConnectAsync();
        var status = canConnect
            ? "✅ Połączono z bazą danych!"
            : "❌ Nie udało się połączyć z bazą danych!";
        logger.LogInformation("GET /  – status połączenia: {Status}", status);

        var html = $@"
          <html>
            <body style='font-family:sans-serif'>
              <h1>{status}</h1>
              <p><a href=""/swagger"">Przejdź do dokumentacji API (Swagger)</a></p>
            </body>
          </html>";
        return Results.Content(html, "text/html; charset=utf-8");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "GET /  – błąd podczas sprawdzania połączenia z bazą");
        return Results.Problem("Wewnętrzny błąd serwera");
    }
});

// 9) Middleware Swagger (po zdefiniowaniu root endpoint)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetProject API v1");
    c.RoutePrefix = "swagger"; // dostęp do Swagger UI pod /swagger
});

// 10) Middleware Authentication / Authorization
app.UseAuthentication();
app.UseAuthorization();

// ────────────────────────────────────────────────────────────────────────────────
// 11) GET /api/customers  – lista klientów (CustomerDTO)
app.MapGet("/api/customers", async (MyAppDbContext db, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("GET /api/customers  – pobieranie listy klientów");
        var customers = await db.Customers.Include(c => c.Vehicles).ToListAsync();
        var dtos = customers.Select(ModelMapper.ToCustomerDTO);
        return Results.Ok(dtos);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "GET /api/customers  – błąd podczas pobierania klientów");
        return Results.Problem("Wewnętrzny błąd serwera");
    }
});

// 12) GET /api/customers/{id}  – pojedynczy klient po ID
app.MapGet("/api/customers/{id:int}", async (int id, MyAppDbContext db, ILogger<Program> logger) =>
{
    try
    {
        // Uwaga: używamy "{{id}}" aby zachować "{id}" literalnie w logu, i "{Id}" jako miejsce na parametr
        logger.LogInformation("GET /api/customers/{{id}}  – pobieranie klienta o id={Id}", id);

        var customer = await db.Customers
                               .Include(c => c.Vehicles)
                               .FirstOrDefaultAsync(c => c.Id == id);
        if (customer == null)
        {
            // Tu ponownie: "{{id}}" to literalna ścieżka, a "{Id}" to placeholder na wartość
            logger.LogWarning("GET /api/customers/{{id}}  – nie znaleziono klienta o id={Id}", id);
            return Results.NotFound(new { message = $"Klient o id={id} nie istnieje." });
        }

        var dto = ModelMapper.ToCustomerDTO(customer);
        return Results.Ok(dto);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "GET /api/customers/{{id}}  – błąd podczas pobierania klienta o id={Id}", id);
        return Results.Problem("Wewnętrzny błąd serwera");
    }
});

// 13) POST /api/customers  – tworzenie nowego klienta
app.MapPost("/api/customers", async (
    NetProject.DTOs.CustomerDTO dto,
    MyAppDbContext db,
    ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("POST /api/customers  – tworzenie klienta");
        var customer = ModelMapper.ToCustomer(dto);
        db.Customers.Add(customer);
        await db.SaveChangesAsync();
        logger.LogInformation("POST /api/customers  – dodano klienta o id={Id}", customer.Id);
        return Results.Created($"/api/customers/{customer.Id}", ModelMapper.ToCustomerDTO(customer));
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "POST /api/customers  – błąd podczas tworzenia klienta");
        return Results.Problem("Wewnętrzny błąd serwera");
    }
});
// ────────────────────────────────────────────────────────────────────────────────

app.Run();
