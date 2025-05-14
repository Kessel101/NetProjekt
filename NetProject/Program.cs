using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using NetProject.Models;
using NetProject.Data;
using NetProject.DTOs;
using NetProject.Mappers;

var builder = WebApplication.CreateBuilder(args);

// 1) DbContext
builder.Services.AddDbContext<MyAppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2) Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MyAppDbContext>()
    .AddDefaultTokenProviders();

// 3) (opcjonalnie) jeśli chcesz seedować Mapperly przy użyciu DI — nie jest to konieczne, bo metody są statyczne
// builder.Services.AddMapperly();  

// 4) Authorization
builder.Services.AddAuthorization();

// 5) Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetProject API", Version = "v1" });
});

var app = builder.Build();

// 6) Seed ról, admina i przykładowych danych
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
await DbInitializer.SeedRolesAndAdminAsync(services);
await DbInitializer.SeedSampleDataAsync(services.GetRequiredService<MyAppDbContext>());

// 7) Swagger middleware (przed auth i endpointami)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetProject API v1");

    c.RoutePrefix = "swagger";
});

// 8) Auth
app.UseAuthentication();
app.UseAuthorization();

// 9) Root endpoint z linkiem do Swagger UI
app.MapGet("/", async (MyAppDbContext db) =>
{
    var status = await db.Database.CanConnectAsync()
        ? "Połączono z bazą danych!"
        : "Nie udało się połączyć z bazą!";
    var html = $@"
      <html><body style='font-family:sans-serif'>
        <h1>{status}</h1>
        <p><a href=""/swagger"">Przejdź do dokumentacji API (Swagger)</a></p>
      </body></html>";
    return Results.Content(html, "text/html; charset=utf-8");
});

// 10) Endpoint GET /api/customers z użyciem ModelMapper
app.MapGet("/api/customers", async (MyAppDbContext db) =>
{
    var customers = await db.Customers.Include(c => c.Vehicles).ToListAsync();
    // statyczna metoda wygenerowana przez Mapperly
    var dtos = customers.Select(ModelMapper.ToCustomerDTO);
    return Results.Ok(dtos);
});

// 11) Uruchomienie
app.Run();
