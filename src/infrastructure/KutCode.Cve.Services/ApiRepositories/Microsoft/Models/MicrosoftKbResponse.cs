using System.Text.Json.Serialization;

namespace KutCode.Cve.Services.ApiRepositories.Microsoft.Models;

public sealed class MicrosoftKbResponse
{
    [JsonPropertyName("value")]
    public List<MicrosoftKbValueItem> Value { get; set; }
}