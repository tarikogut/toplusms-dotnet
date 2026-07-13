using System.ComponentModel.DataAnnotations;

namespace Toplusms.Models;

public class LineStatus
{
    [Key]
    public byte Code { get; set; }

    [Required, MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
