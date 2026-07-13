using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Toplusms.Models;

public enum AddressType
{
    TESIS,
    FATURA,
    YERLESIM
}

public class TenantAddress
{
    [Key]
    public uint Id { get; set; }

    public uint TenantId { get; set; }

    [MaxLength(20)]
    public string Type { get; set; } = "TESIS";

    [MaxLength(100)]
    public string Country { get; set; } = "TUR";

    [MaxLength(100)]
    public string Province { get; set; } = string.Empty;

    [MaxLength(100)]
    public string District { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Neighborhood { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Street { get; set; } = string.Empty;

    [MaxLength(20)]
    public string BuildingNo { get; set; } = string.Empty;

    [MaxLength(20)]
    public string ApartmentNo { get; set; } = string.Empty;

    [MaxLength(20)]
    public string PostalCode { get; set; } = string.Empty;

    [MaxLength(20)]
    public string AddressCode { get; set; } = string.Empty;

    [MaxLength(500)]
    public string FullAddress { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(TenantId))]
    public Tenant? Tenant { get; set; }
}
