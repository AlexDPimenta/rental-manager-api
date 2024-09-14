using System.Text.Json.Serialization;

namespace RentalManager.WebApi.Contracts.MotorCycles
{
    public class UpdatePlateByIdResponse
    {
        [JsonPropertyName("mensagem")]
        public string Message { get; set; }
    }
}
