using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NetProject.Models;
using NetProject.Data;
using NetProject.Mappers; // Dodanie przestrzeni nazw do mapowania
using NetProject.DTOs;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja DbContext
var cs = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyAppDbContext>(opt =>
    opt.UseSqlServer(cs));

// Rejestracja Identity (tożsamości i ról)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MyAppDbContext>()
    .AddDefaultTokenProviders();

// Dodanie autoryzacji
builder.Services.AddAuthorization();

var app = builder.Build();

// Inicjalizacja ról i admina, teraz po zarejestrowaniu usług Identity
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedRolesAndAdminAsync(services);
    
    var context = services.GetRequiredService<MyAppDbContext>();
    await DbInitializer.SeedSampleDataAsync(context);
}

// TEST: Endpoint, który sprawdzi połączenie z DB
app.MapGet("/", async (MyAppDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return canConnect ? "Połączono z bazą danych!" : "Brak połączenia!";
});

// Endpoint, który zwróci klientów jako DTO
app.MapGet("/api/customers", async (MyAppDbContext db) =>
{
    // Pobieramy klientów z bazy
    var customers = await db.Customers.ToListAsync();

    // Mapujemy klientów na DTO za pomocą ModelMapper
    var customerDTOs = customers.Select(c => ModelMapper.ToCustomerDTO(c)).ToList();

    // Zwracamy mapowane dane
    return Results.Ok(customerDTOs);
});

// Middleware do autoryzacji
app.UseAuthentication();
app.UseAuthorization(); // Użycie autoryzacji

// Uruchomienie aplikacji
app.Run();