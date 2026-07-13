using System.ComponentModel.DataAnnotations;

namespace Toplusms.Models;

public class Role
{
    [Key]
    public uint Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public string PermissionsJson { get; set; } = "[]";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<User> Users { get; set; } = new List<User>();
}
