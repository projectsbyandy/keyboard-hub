using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VendorApi.Models.Vendor;

public record VendorDetails
{
    [JsonPropertyName("id")]
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
    
    [JsonPropertyName("name")]
    [MaxLength(20)]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    
    [JsonPropertyName("yearsActive")]
    public int YearsActive { get; set; }
    
    [JsonPropertyName("isLive")]
    public bool IsLive { get; set; }
}