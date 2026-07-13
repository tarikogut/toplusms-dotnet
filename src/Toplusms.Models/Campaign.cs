using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Toplusms.Models;

public class Campaign
{
    [Key]
    public uint Id { get; set; }
    public uint TenantId { get; set; }
    public uint UserId { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "draft";

    public DateTime? SentAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(TenantId))]
    public Tenant? Tenant { get; set; }
}
