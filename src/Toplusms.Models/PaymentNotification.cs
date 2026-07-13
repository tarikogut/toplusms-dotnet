using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Toplusms.Models;

public class PaymentNotification
{
    [Key]
    public uint Id { get; set; }
    public uint TenantId { get; set; }
    public uint UserId { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    [MaxLength(50)]
    public string Method { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "pending";

    public string Notes { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(TenantId))]
    public Tenant? Tenant { get; set; }
}
