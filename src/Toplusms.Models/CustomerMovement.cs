using System.ComponentModel.DataAnnotations;

namespace Toplusms.Models;

public class CustomerMovement
{
    [Key]
    public byte Code { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
