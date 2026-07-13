using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Toplusms.Models;

public class Contact
{
    [Key]
    public uint Id { get; set; }
    public uint TenantId { get; set; }
    public uint UserId { get; set; }

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    public uint? GroupId { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "active";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(TenantId))]
    public Tenant? Tenant { get; set; }
    public Group? Group { get; set; }
}
