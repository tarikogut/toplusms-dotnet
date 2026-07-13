using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Toplusms.Models;

public class SMSMessage
{
    [Key]
    public uint Id { get; set; }
    public uint TenantId { get; set; }
    public uint UserId { get; set; }

    [MaxLength(100)]
    public string Header { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public string RecipientsJson { get; set; } = "[]";

    public int TotalRecipients { get; set; }
    public int SentCount { get; set; }
    public int FailedCount { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "pending"; // pending|sent|failed|partial

    public DateTime? SentAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(TenantId))]
    public Tenant? Tenant { get; set; }
}
