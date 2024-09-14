namespace RentalManager.WebApi.Entities;

public class Driver
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Cnpj { get; set; }
    public DateOnly BirthdayDate { get; set; }
    public string LicenseNumber { get; set; }
    public string LicenseCategory { get; set; }
    public string LicenseImage { get; set; }   
    public IEnumerable<Lease> Leases { get; set; }
}
