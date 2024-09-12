using System.Text.Json.Serialization;

namespace RentalManager.WebApi.Contracts.MotorCycles;

public class MotorCycleResponse
{
    [JsonPropertyName("Identificador")]    
    public string Id { get; set; }
    [JsonPropertyName("Ano")]   
    public int Year { get; set; }
    [JsonPropertyName("Modelo")]   
    public string Model { get; set; }
    [JsonPropertyName("Placa")]   
    public string Plate { get; set; }

}
