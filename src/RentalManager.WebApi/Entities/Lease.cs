namespace RentalManager.WebApi.Entities;

public class Lease
{
    public Guid Id { get; set; }
    public string MotorCycleId { get; set; }
    public string DriverId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public int DurationInDays { get; set; }

    public Plan LeasePlan { get; set; }
    public MotorCycle MotorCycle { get; set; }
    public Driver Driver { get; set; }
}
