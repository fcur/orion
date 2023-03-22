using System.Text.Json.Serialization;

namespace Orion.App.Integration.SiriusProvider.Dto;

public sealed class EventContentDto
{
    [JsonPropertyName("id")] 
    public string Id { get; set; } = null!;
    
    [JsonPropertyName("status")] 
    public string Status { get; set; } = null!;
    
    [JsonPropertyName("type")] 
    public string ContentType { get; set; } = null!;
    
    [JsonPropertyName("locale")] 
    public string Locale { get; set; } = null!;
}