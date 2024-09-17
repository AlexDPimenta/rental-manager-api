using System.Text.Json.Serialization;

namespace RentalManager.WebApi.Contracts.Leases;

public class UpdateLeaseRequest
{
    [JsonPropertyName("data_devolucao")]
    public DateTime ReturnData { get; set; }
}
