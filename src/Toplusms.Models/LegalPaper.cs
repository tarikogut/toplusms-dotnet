using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Toplusms.Models;

public class LegalPaper
{
    [Key]
    public uint Id { get; set; }
    public uint TenantId { get; set; }

    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(255)]
    public string FilePath { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(TenantId))]
    public Tenant? Tenant { get; set; }
}
