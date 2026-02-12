using Celsia.TechVisits.Api.Controllers;
using Celsia.TechVisits.Api.Data;
using Celsia.TechVisits.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Celsia.TechVisits.Api.Tests;

public class AppointmentsControllerTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly AppointmentsController _controller;

    public AppointmentsControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _controller = new AppointmentsController(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task GetAppointments_ReturnsEmptyList_WhenNoAppointmentsExist()
    {
        // Act
        var result = await _controller.GetAppointments();

        // Assert
        var appointments = Assert.IsAssignableFrom<IEnumerable<Appointment>>(result.Value);
        Assert.Empty(appointments);
    }

    [Fact]
    public async Task GetAppointments_ReturnsAllAppointments_WithCustomerIncluded()
    {
        // Arrange
        var customer = new Customer { Nic = "111111", Name = "Test User" };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        _context.Appointments.Add(new Appointment
        {
            Date = DateTime.Today.AddDays(1),
            TimeSlot = "AM",
            Status = "Scheduled",
            CustomerId = customer.Id
        });
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetAppointments();

        // Assert
        var appointments = Assert.IsAssignableFrom<IEnumerable<Appointment>>(result.Value);
        Assert.Single(appointments);
        Assert.Equal("Test User", appointments.First().Customer.Name);
    }

    [Fact]
    public async Task GetAppointmentsByNic_ReturnsEmpty_WhenNicHasNoAppointments()
    {
        // Act
        var result = await _controller.GetAppointmentsByNic("999999");

        // Assert
        var appointments = Assert.IsAssignableFrom<IEnumerable<Appointment>>(result.Value);
        Assert.Empty(appointments);
    }

    [Fact]
    public async Task GetAppointmentsByNic_ReturnsAppointments_ForExistingCustomer()
    {
        // Arrange
        var customer = new Customer { Nic = "222222", Name = "Maria Test" };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        _context.Appointments.AddRange(
            new Appointment { Date = DateTime.Today.AddDays(1), TimeSlot = "AM", Status = "Scheduled", CustomerId = customer.Id },
            new Appointment { Date = DateTime.Today.AddDays(2), TimeSlot = "PM", Status = "Scheduled", CustomerId = customer.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetAppointmentsByNic("222222");

        // Assert
        var appointments = Assert.IsAssignableFrom<IEnumerable<Appointment>>(result.Value);
        Assert.Equal(2, appointments.Count());
    }

    [Fact]
    public async Task GetAppointmentsByNic_ReturnsOrderedByDateDesc()
    {
        // Arrange
        var customer = new Customer { Nic = "333333", Name = "Order Test" };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        _context.Appointments.AddRange(
            new Appointment { Date = DateTime.Today.AddDays(1), TimeSlot = "AM", Status = "Scheduled", CustomerId = customer.Id },
            new Appointment { Date = DateTime.Today.AddDays(5), TimeSlot = "PM", Status = "Scheduled", CustomerId = customer.Id }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetAppointmentsByNic("333333");

        // Assert
        var appointments = Assert.IsAssignableFrom<IEnumerable<Appointment>>(result.Value).ToList();
        Assert.True(appointments[0].Date > appointments[1].Date);
    }

    [Fact]
    public async Task CreateAppointment_WithNewCustomer_CreatesCustomerAndAppointment()
    {
        // Arrange
        var appointment = new Appointment
        {
            Date = DateTime.Today.AddDays(3),
            TimeSlot = "AM",
            Status = "Scheduled",
            Customer = new Customer { Nic = "444444", Name = "New Customer" }
        };

        // Act
        var result = await _controller.CreateAppointment(appointment);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var created = Assert.IsType<Appointment>(createdResult.Value);
        Assert.Equal("Scheduled", created.Status);
        Assert.True(created.Id > 0);

        // Verify customer was created
        var savedCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Nic == "444444");
        Assert.NotNull(savedCustomer);
        Assert.Equal("New Customer", savedCustomer.Name);
    }

    [Fact]
    public async Task CreateAppointment_WithExistingCustomer_LinkesToExisting()
    {
        // Arrange
        var existingCustomer = new Customer { Nic = "555555", Name = "Existing Customer" };
        _context.Customers.Add(existingCustomer);
        await _context.SaveChangesAsync();

        var appointment = new Appointment
        {
            Date = DateTime.Today.AddDays(4),
            TimeSlot = "PM",
            Status = "Scheduled",
            Customer = new Customer { Nic = "555555", Name = "Different Name" }
        };

        // Act
        var result = await _controller.CreateAppointment(appointment);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var created = Assert.IsType<Appointment>(createdResult.Value);
        Assert.Equal(existingCustomer.Id, created.CustomerId);

        // Verify no duplicate customer
        var customerCount = await _context.Customers.CountAsync(c => c.Nic == "555555");
        Assert.Equal(1, customerCount);
    }

    [Fact]
    public async Task CreateAppointment_SetsCorrectTimeSlot()
    {
        // Arrange
        var appointment = new Appointment
        {
            Date = DateTime.Today.AddDays(5),
            TimeSlot = "PM",
            Status = "Scheduled",
            Customer = new Customer { Nic = "666666", Name = "PM Customer" }
        };

        // Act
        var result = await _controller.CreateAppointment(appointment);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var created = Assert.IsType<Appointment>(createdResult.Value);
        Assert.Equal("PM", created.TimeSlot);
    }
}
