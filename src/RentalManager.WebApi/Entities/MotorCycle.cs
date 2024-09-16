namespace RentalManager.WebApi.Entities;

public class MotorCycle
{
    public string Id { get; set; }
    public int Year { get; set; }
    public string Model { get; set; }
    public string Plate { get; set; }    

    public IEnumerable<Lease> Leases { get; init; } = new List<Lease>();
}
