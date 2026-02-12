using Celsia.TechVisits.Api.Controllers;
using Celsia.TechVisits.Api.Data;
using Celsia.TechVisits.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Celsia.TechVisits.Api.Tests;

public class CustomersControllerTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly CustomersController _controller;

    public CustomersControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _controller = new CustomersController(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task GetCustomers_ReturnsEmptyList_WhenNoCustomersExist()
    {
        // Act
        var result = await _controller.GetCustomers();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Customer>>>(result);
        var customers = Assert.IsAssignableFrom<IEnumerable<Customer>>(actionResult.Value);
        Assert.Empty(customers);
    }

    [Fact]
    public async Task GetCustomers_ReturnsAllCustomers()
    {
        // Arrange
        _context.Customers.AddRange(
            new Customer { Nic = "111111", Name = "Ana Gomez" },
            new Customer { Nic = "222222", Name = "Carlos Lopez" }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetCustomers();

        // Assert
        var customers = Assert.IsAssignableFrom<IEnumerable<Customer>>(result.Value);
        Assert.Equal(2, customers.Count());
    }

    [Fact]
    public async Task GetCustomer_ReturnsNotFound_WhenIdDoesNotExist()
    {
        // Act
        var result = await _controller.GetCustomer(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetCustomer_ReturnsCustomer_WhenIdExists()
    {
        // Arrange
        var customer = new Customer { Nic = "333333", Name = "Maria Ruiz" };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetCustomer(customer.Id);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal("333333", result.Value.Nic);
        Assert.Equal("Maria Ruiz", result.Value.Name);
    }

    [Fact]
    public async Task GetCustomerByNic_ReturnsNotFound_WhenNicDoesNotExist()
    {
        // Act
        var result = await _controller.GetCustomerByNic("999999");

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetCustomerByNic_ReturnsCustomer_WhenNicExists()
    {
        // Arrange
        var customer = new Customer { Nic = "444444", Name = "Pedro Diaz" };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetCustomerByNic("444444");

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal("Pedro Diaz", result.Value.Name);
    }

    [Fact]
    public async Task CreateCustomer_ReturnsCreatedAtAction()
    {
        // Arrange
        var customer = new Customer { Nic = "555555", Name = "Laura Torres" };

        // Act
        var result = await _controller.CreateCustomer(customer);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var created = Assert.IsType<Customer>(createdResult.Value);
        Assert.Equal("555555", created.Nic);
        Assert.True(created.Id > 0);
    }

    [Fact]
    public async Task CreateCustomer_PersistsToDatabase()
    {
        // Arrange
        var customer = new Customer { Nic = "666666", Name = "Sofia Mendez" };

        // Act
        await _controller.CreateCustomer(customer);

        // Assert
        var saved = await _context.Customers.FirstOrDefaultAsync(c => c.Nic == "666666");
        Assert.NotNull(saved);
        Assert.Equal("Sofia Mendez", saved.Name);
    }
}
