using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Celsia.TechVisits.Api.Data;
using Celsia.TechVisits.Api.Models;

namespace Celsia.TechVisits.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        return await _context.Customers.ToListAsync();
    }

    // GET: api/Customers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
        {
            return NotFound();
        }

        return customer;
    }

    // GET: api/Customers/nic/12345
    [HttpGet("nic/{nic}")]
    public async Task<ActionResult<Customer>> GetCustomerByNic(string nic)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Nic == nic);

        if (customer == null)
        {
            return NotFound();
        }

        return customer;
    }

    // POST: api/Customers
    [HttpPost]
    public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
    {
        if (_context.Customers.Any(c => c.Nic == customer.Nic))
        {
            return Conflict($"Customer with NIC {customer.Nic} already exists.");
        }

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
    }
}
