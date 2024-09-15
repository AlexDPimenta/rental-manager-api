using System.Text.Json.Serialization;

namespace RentalManager.WebApi.Contracts.Drivers;

public class AddLicenseImageRequest
{
    [JsonPropertyName("imagem_cnh")]
    [JsonRequired]
    public string LicenseImage { get; set; }
}
