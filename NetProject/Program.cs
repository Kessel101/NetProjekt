using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NetProject.Data;
using NetProject.Models;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// ─── NLog ───────────────────────────────────────────────────────────────────────
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// ─── DbContext + Identity + UI ─────────────────────────────────────────────────
builder.Services.AddDbContext<MyAppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MyAppDbContext>();

// Razor Pages (Identity UI)
builder.Services.AddRazorPages();

// MVC Controllers + Views (dla AdminController i innych)
builder.Services.AddControllersWithViews();

// ─── Swagger (do testów API) ────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetProject API", Version = "v1" });
});

var app = builder.Build();

// ─── Seed ról i admina ──────────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedRolesAndAdminAsync(services);
}

// ─── Middleware Swagger ────────────────────────────────────────────────────────
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetProject API v1");
    c.RoutePrefix = "swagger";
});

// ─── Middleware uwierzytelniania i autoryzacji ─────────────────────────────────
app.UseAuthentication();
app.UseAuthorization();

// ─── Mapowanie tras MVC ─────────────────────────────────────────────────────────
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// ─── Mapowanie Razor Pages (Identity UI) ────────────────────────────────────────
app.MapRazorPages();

// ─── Root: przekierowanie na stronę logowania lub Manage ───────────────────────
app.MapGet("/", (HttpContext ctx) =>
{
    if (ctx.User.Identity?.IsAuthenticated ?? false)
    {
        return Results.Redirect("/Identity/Account/Manage");
    }
    else
    {
        return Results.Redirect("/Identity/Account/Login");
    }
});

app.Run();
