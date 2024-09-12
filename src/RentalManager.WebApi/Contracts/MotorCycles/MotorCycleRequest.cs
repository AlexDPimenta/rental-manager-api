using MediatR;
using RentalManager.WebApi.Common;
using System.Text.Json.Serialization;

namespace RentalManager.WebApi.Contracts.MotorCycles;

public sealed class MotorCycleRequest: IRequest<Result<MotorCycleResponse>>
{
    [JsonPropertyName("Identificador")]
    [JsonRequired]
    public string Id { get; set; }
    [JsonPropertyName("Ano")]
    [JsonRequired]
    public int Year { get; set; }
    [JsonPropertyName("Modelo")]
    [JsonRequired]
    public string Model { get; set; }
    [JsonPropertyName("Placa")]
    [JsonRequired]
    public string Plate { get; set; }
}
