using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Celsia.TechVisits.Api.Data;
using Celsia.TechVisits.Api.Models;

namespace Celsia.TechVisits.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AppointmentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
    {
        return await _context.Appointments
            .Include(a => a.Customer)
            .OrderByDescending(a => a.Date)
            .ToListAsync();
    }

    // GET: api/Appointments/nic/123456
    [HttpGet("nic/{nic}")]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByNic(string nic)
    {
        var appointments = await _context.Appointments
            .Include(a => a.Customer)
            .Where(a => a.Customer.Nic == nic)
            .OrderByDescending(a => a.Date)
            .ToListAsync();

        return appointments;
    }

    [HttpPost]
    public async Task<ActionResult<Appointment>> CreateAppointment(Appointment appointment)
    {
        if (appointment.Customer != null && !string.IsNullOrEmpty(appointment.Customer.Nic))
        {
            // Check if customer exists by NIC
            var existingCustomer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Nic == appointment.Customer.Nic);
                
            if (existingCustomer != null)
            {
                // Attach existing customer
                appointment.Customer = null!; 
                appointment.CustomerId = existingCustomer.Id;
            }
            // If not exists, EF Core will create the new Customer from the navigation property
        }
        else if (appointment.CustomerId == 0)
        {
            return BadRequest("Customer information (NIC) or CustomerId is required.");
        }

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        // Reload to get the Customer data populated in response
        await _context.Entry(appointment).Reference(a => a.Customer).LoadAsync();

        return CreatedAtAction(nameof(GetAppointments), new { id = appointment.Id }, appointment);
    }
}
