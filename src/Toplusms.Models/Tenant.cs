using System.ComponentModel.DataAnnotations;

namespace Toplusms.Models;

public class Tenant
{
    [Key]
    public uint Id { get; set; }

    [Required, MaxLength(255)]
    public string Domain { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Type { get; set; } = "host"; // host|bayi|musteri

    [MaxLength(20)]
    public string Status { get; set; } = "active";

    public uint? ParentTenantId { get; set; }

    // BTK / vergi bilgileri
    [MaxLength(50)]
    public string TaxNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string TaxOffice { get; set; } = string.Empty;

    [MaxLength(11)]
    public string IdentityNumber { get; set; } = string.Empty;

    [MaxLength(20)]
    public string CustomerType { get; set; } = "B"; // B=bireysel, T=tuzel

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    public string SettingsJson { get; set; } = "{}";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Tenant? ParentTenant { get; set; }
    public ICollection<Tenant> Children { get; set; } = new List<Tenant>();
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<TenantAddress> Addresses { get; set; } = new List<TenantAddress>();
}
