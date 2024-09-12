namespace RentalManager.WebApi.Events;

public class EventsBase
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
