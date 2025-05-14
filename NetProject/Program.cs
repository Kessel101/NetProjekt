using Microsoft.EntityFrameworkCore;
using NetProject.Data;

var builder = WebApplication.CreateBuilder(args);

var cs = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MyAppDbContext>(opt =>
    opt.UseSqlServer(cs));

var app = builder.Build();

// TEST: Endpoint, który sprawdzi połączenie z DB
app.MapGet("/", async (MyAppDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return canConnect ? "Połączono z bazą danych!" : "Brak połączenia!";
});

app.Run();