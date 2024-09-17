using System.Text.Json.Serialization;

namespace RentalManager.WebApi.Contracts.Leases;

public class LeaseResponse
{
    [JsonPropertyName("identificador")]
    public string Id { get; set; }
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
    [JsonPropertyName("valor_diaria")]
    public decimal CostPerDay { get; set; }
    [JsonPropertyName("data_devolucao")]
    public DateTime? ReturnData { get; set; }
}
