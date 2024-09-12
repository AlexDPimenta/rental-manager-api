namespace RentalManager.WebApi.Events;

public class MotorCycleCreated: EventsBase
{
    public string Id { get; set; }
    public int Year { get; set; }
    public string Model { get; set; }
    public string Plate { get; set; }    
}
