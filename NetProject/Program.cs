using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NetProject.Data;
using NetProject.Models;
using NetProject.DTOs; // <-- tutaj wczytujesz DTO
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// NLog konfiguracja
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// DbContext + Identity
builder.Services.AddDbContext<MyAppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
})
.AddEntityFrameworkStores<MyAppDbContext>()
.AddDefaultTokenProviders();

// Dodaj uwierzytelnianie i autoryzację
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// Swagger (do testów API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetProject API", Version = "v1" });
});

var app = builder.Build();

// Seed ról/admina jeśli masz metodę
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
await DbInitializer.SeedRolesAndAdminAsync(services);

// Middleware Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetProject API v1");
    c.RoutePrefix = "swagger";
});

// Middleware uwierzytelniania i autoryzacji
app.UseAuthentication();
app.UseAuthorization();

// Root przekierowujący na Swaggera
app.MapGet("/", () => Results.Redirect("/swagger"));

// Endpoint rejestracji
app.MapPost("/api/account/register", async (
    RegisterDTO model,
    UserManager<ApplicationUser> userManager,
    ILogger<Program> logger) =>
{
    if (model is null)
        return Results.BadRequest("Brak danych rejestracji");

    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
    var result = await userManager.CreateAsync(user, model.Password);

    if (!result.Succeeded)
    {
        logger.LogWarning("Rejestracja nie powiodła się: {Errors}", string.Join("; ", result.Errors.Select(e => e.Description)));
        return Results.BadRequest(result.Errors.Select(e => e.Description));
    }

    logger.LogInformation("Nowy użytkownik zarejestrowany: {Email}", model.Email);
    return Results.Ok(new { message = "Rejestracja zakończona sukcesem." });
});

// Endpoint logowania
app.MapPost("/api/account/login", async (
    LoginDTO model,
    SignInManager<ApplicationUser> signInManager,
    ILogger<Program> logger) =>
{
    if (model is null)
        return Results.BadRequest("Brak danych logowania");

    var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

    if (!result.Succeeded)
    {
        logger.LogWarning("Nieudane logowanie: {Email}", model.Email);
        return Results.BadRequest(new { message = "Niepoprawny email lub hasło." });
    }

    logger.LogInformation("Użytkownik zalogowany: {Email}", model.Email);
    return Results.Ok(new { message = "Zalogowano pomyślnie." });
});

app.Run();
