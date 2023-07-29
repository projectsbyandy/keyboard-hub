using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeyboardAPI.Models;

public class Vendor
{
    [Key] public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; }
    [NotMapped]
    public List<string> Brands { get; set; } = new();
}