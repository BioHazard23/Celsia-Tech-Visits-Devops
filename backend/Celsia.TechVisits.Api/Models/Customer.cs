namespace Celsia.TechVisits.Api.Models;

public class Customer
{
    public int Id { get; set; }
    public string Nic { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public List<Appointment> Appointments { get; set; } = new();
}
