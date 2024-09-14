using RentalManager.WebApi.Entities;
using System.Text.Json.Serialization;

namespace RentalManager.WebApi.Contracts.Drivers;

public class AddDriverRequest
{
    [JsonPropertyName("identificador")]
    [JsonRequired]
    public string Id { get; set; }
    [JsonPropertyName("nome")]
    [JsonRequired]
    public string Name { get; set; }
    [JsonPropertyName("cnpj")]
    [JsonRequired]
    public string Cnpj { get; set; }
    [JsonPropertyName("data_nascimento")]
    [JsonRequired]
    public DateTime BirthdayDate { get; set; }
    [JsonPropertyName("numero_cnh")]
    [JsonRequired]
    public string LicenseNumber { get; set; }
    [JsonPropertyName("tipo_cnh")]
    [JsonRequired]
    public string LicenseCategory { get; set; }
    [JsonPropertyName("imagem_cnh")]
    public string LicenseImage { get; set; }
}
