using Microsoft.EntityFrameworkCore;
using Celsia.TechVisits.Api.Models;

namespace Celsia.TechVisits.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
}
