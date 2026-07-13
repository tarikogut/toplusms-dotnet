using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Toplusms.Models;

public class User
{
    [Key]
    public uint Id { get; set; }

    public uint TenantId { get; set; }

    [Required, MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Surname { get; set; } = string.Empty;

    [MaxLength(11)]
    public string TcNo { get; set; } = string.Empty;

    public uint RoleId { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "active";

    [MaxLength(255)]
    public string? TwoFaSecret { get; set; }

    public bool TwoFaEnabled { get; set; }

    [Column(TypeName = "decimal(10,4)")]
    public decimal UnitPrice { get; set; }

    public uint? ParentUserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(TenantId))]
    public Tenant? Tenant { get; set; }

    [ForeignKey(nameof(RoleId))]
    public Role? Role { get; set; }

    [ForeignKey(nameof(ParentUserId))]
    public User? ParentUser { get; set; }

    public ICollection<User> Children { get; set; } = new List<User>();
}
