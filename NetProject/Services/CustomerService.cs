using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetProject.Data;
using NetProject.Models;

namespace NetProject.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
    }

    public class CustomerService : ICustomerService
    {
        private readonly MyAppDbContext _db;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(MyAppDbContext db, ILogger<CustomerService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("CustomerService.GetAllAsync() - pobieranie wszystkich klientów");
                return await _db.Customers.Include(c => c.Vehicles).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w CustomerService.GetAllAsync()");
                throw; // lub obsłuż inaczej
            }
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("CustomerService.GetByIdAsync({Id})", id);
                return await _db.Customers.Include(c => c.Vehicles)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w CustomerService.GetByIdAsync({Id})", id);
                throw;
            }
        }
    }
}