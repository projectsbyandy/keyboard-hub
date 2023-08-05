#nullable disable
using System.ComponentModel.DataAnnotations;

namespace KeyboardApi.Models;

public class Vendor
{
    [Key] public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; }
    public int YearsActive { get; set; }
    public bool Live { get; set; }
}