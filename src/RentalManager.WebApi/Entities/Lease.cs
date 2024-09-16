namespace RentalManager.WebApi.Entities;

public class Lease
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string MotorCycleId { get; set; }
    public string DriverId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public DateTime? ReturnData { get; set; }
    public int DurationInDays { get; set; }

    public Plan LeasePlan { get; set; }
    public MotorCycle MotorCycle { get; set; }
    public Driver Driver { get; set; }
}
