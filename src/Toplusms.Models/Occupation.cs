using System.ComponentModel.DataAnnotations;

namespace Toplusms.Models;

public class Occupation
{
    [Key]
    public uint Code { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
