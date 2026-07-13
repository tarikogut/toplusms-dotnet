using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Toplusms.Models;

public class BankAccount
{
    [Key]
    public uint Id { get; set; }
    public uint TenantId { get; set; }

    [MaxLength(255)]
    public string BankName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string BranchCode { get; set; } = string.Empty;

    [MaxLength(50)]
    public string AccountNumber { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Iban { get; set; } = string.Empty;

    [MaxLength(255)]
    public string HolderName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(TenantId))]
    public Tenant? Tenant { get; set; }
}
