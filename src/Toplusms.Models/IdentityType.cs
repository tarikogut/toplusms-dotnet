using System.ComponentModel.DataAnnotations;

namespace Toplusms.Models;

public class IdentityType
{
    [Key, MaxLength(10)]
    public string Code { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(5)]
    public string CountryCode { get; set; } = string.Empty;

    public bool HasSerialNo { get; set; }

    [MaxLength(10)]
    public string IdentityNoField { get; set; } = string.Empty; // TCKN, YKN, OPTIONAL

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
