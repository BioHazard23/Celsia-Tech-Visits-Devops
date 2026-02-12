namespace Celsia.TechVisits.Api.Models;

public class Appointment
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string TimeSlot { get; set; } = string.Empty;
    public string Status { get; set; } = "Scheduled";

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
}
