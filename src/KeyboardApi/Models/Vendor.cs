#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KeyboardApi.Models;

public class Vendor
{
    [JsonPropertyName("Id")]
    [Key] public Guid Id { get; init; } = Guid.NewGuid();
    
    [JsonPropertyName("Name")]
    public string Name { get; set; }
    
    [JsonPropertyName("YearsActive")]
    public int YearsActive { get; set; }
    
    [JsonPropertyName("Live")]
    public bool Live { get; set; }
}