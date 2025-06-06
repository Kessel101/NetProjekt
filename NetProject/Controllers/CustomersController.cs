using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetProject.Data;
using NetProject.Mappers;
using NetProject.DTOs;

namespace NetProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly MyAppDbContext _db;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(MyAppDbContext db, ILogger<CustomersController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Wywołanie GET /api/customers");
                var customers = await _db.Customers.Include(c => c.Vehicles).ToListAsync();
                var dtos = customers.Select(ModelMapper.ToCustomerDTO);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                // Zaloguj błąd, a potem zwróć status 500
                _logger.LogError(ex, "Błąd podczas pobierania listy klientów");
                return StatusCode(500, "Wewnętrzny błąd serwera");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Wywołanie GET /api/customers/{id} (id={Id})", id);
                var customer = await _db.Customers.Include(c => c.Vehicles)
                                                  .FirstOrDefaultAsync(c => c.Id == id);
                if (customer == null)
                {
                    _logger.LogWarning("Nie znaleziono klienta o id {Id}", id);
                    return NotFound();
                }
                return Ok(ModelMapper.ToCustomerDTO(customer));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania klienta o id {Id}", id);
                return StatusCode(500, "Wewnętrzny błąd serwera");
            }
        }

        // Przykład POST
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerDTO dto)
        {
            try
            {
                _logger.LogInformation("Wywołanie POST /api/customers");
                var customer = ModelMapper.ToCustomer(dto);
                _db.Customers.Add(customer);
                await _db.SaveChangesAsync();
                _logger.LogInformation("Dodano nowego klienta (id={Id})", customer.Id);
                return CreatedAtAction(nameof(GetById), new { id = customer.Id }, ModelMapper.ToCustomerDTO(customer));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas tworzenia klienta");
                return StatusCode(500, "Wewnętrzny błąd serwera");
            }
        }
    }
}
