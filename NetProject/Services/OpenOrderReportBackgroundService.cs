using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetProject.Data;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using NetProject.Services.PdfDocuments;
using NetProject.ViewModels;

public class EmailReportOptions
{
    public string AdminEmail { get; set; }
    public int IntervalSeconds { get; set; } = 120;
    public string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpUser { get; set; }
    public string SmtpPass { get; set; }
}

public class OpenOrderReportBackgroundService : BackgroundService
{
    private readonly MyAppDbContext _db;
    private readonly EmailReportOptions _opts;
    private readonly ILogger<OpenOrderReportBackgroundService> _log;

    public OpenOrderReportBackgroundService(
        MyAppDbContext db,
        IOptions<EmailReportOptions> opts,
        ILogger<OpenOrderReportBackgroundService> log)
    {
        _db = db;
        _opts = opts.Value;
        _log = log;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _log.LogInformation("Tło zapoczątkowane.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await GenerateAndSendReport();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Błąd podczas wysyłania raportu");
            }

            await Task.Delay(TimeSpan.FromSeconds(_opts.IntervalSeconds), stoppingToken);
        }
    }

    private async Task GenerateAndSendReport()
    {
        var open = await _db.WorkOrders
            .Include(o => o.Customer)
            .Include(o => o.Vehicle)
            .Where(o => o.Status != "Zamknięte")
            .ToListAsync();

        var vm = new MonthlySummaryReportViewModel
        {
            Month = DateTime.UtcNow.Month,
            Year = DateTime.UtcNow.Year,
            Items = open.Select(o => new MonthlySummaryItem
            {
                CustomerName = o.Customer.FullName,
                VehicleReg   = o.Vehicle.RegistrationNumber,
                OrderCount   = 1,
                TotalCost    = o.ServiceTasks.Sum(t =>
                    t.LaborCost + t.ServiceTaskParts.Sum(sp => sp.Part.UnitPrice * sp.Quantity))
            }).ToList()
        };

        var pdf = new MonthlySummaryDocument(vm).GeneratePdf();


        var path = Path.Combine(AppContext.BaseDirectory, "open_orders.pdf");
        await File.WriteAllBytesAsync(path, pdf);

        using var msg = new MailMessage("no-reply@" + _opts.SmtpHost, _opts.AdminEmail)
        {
            Subject = $"Raport otwartych zleceń {DateTime.UtcNow:yyyy-MM-dd}",
            Body = "W załączniku aktualny raport."
        };
        msg.Attachments.Add(new Attachment(path));

        using var smtp = new SmtpClient(_opts.SmtpHost, _opts.SmtpPort)
        {
            Credentials = new NetworkCredential(_opts.SmtpUser, _opts.SmtpPass),
            EnableSsl = true
        };

        await smtp.SendMailAsync(msg);
        _log.LogInformation("Raport wysłano: {email}", _opts.AdminEmail);
    }
}
