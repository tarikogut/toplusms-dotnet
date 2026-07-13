using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Toplusms.Models;

public class Group
{
    [Key]
    public uint Id { get; set; }
    public uint TenantId { get; set; }
    public uint UserId { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(TenantId))]
    public Tenant? Tenant { get; set; }

    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
}
