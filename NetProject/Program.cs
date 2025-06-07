using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

// ─── MVC + Razor Pages ───────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();  // kontrolery MVC + widoki
builder.Services.AddRazorPages();            // Identity UI (Areas/Identity)

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

// ─── Middleware ─────────────────────────────────────────────────────────────────
app.UseStaticFiles();        // aby działał ~/lib/bootstrap/css
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetProject API v1");
    c.RoutePrefix = "swagger";
});

app.UseAuthentication();
app.UseAuthorization();

// ─── Mapowanie tras ─────────────────────────────────────────────────────────────
// MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// Razor Pages (Identity UI)
app.MapRazorPages();

// Root → przekierowanie do panelu lub logowania
app.MapGet("/", (HttpContext ctx) =>
    Results.Redirect(ctx.User.Identity?.IsAuthenticated == true
        ? "/Identity/Account/Manage"
        : "/Identity/Account/Login"));

app.Run();
