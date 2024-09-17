using System.Text.Json.Serialization;

namespace RentalManager.WebApi.Contracts.Leases;

public class LeaseRequest
{
    [JsonPropertyName("entregador_id")]
    public string DriverId { get; set; }
    [JsonPropertyName("moto_id")]
    public string MotorCycleId { get; set; }
    [JsonPropertyName("data_inicio")]
    public DateTime StartDate { get; set; }
    [JsonPropertyName("data_termino")]
    public DateTime EndDate { get; set; }
    [JsonPropertyName("data_previsao_termino")]
    public DateTime ExpectedEndDate { get; set; }
    [JsonPropertyName("plano")]
    public int DurationInDays { get; set; }
}
