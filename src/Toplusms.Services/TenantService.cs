using Microsoft.EntityFrameworkCore;
using Toplusms.Data;
using Toplusms.Models;

namespace Toplusms.Services;

public class TenantService
{
    private readonly AppDbContext _db;

    public TenantService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Tenant>> GetVisibleTenantsAsync(uint currentTenantId, string tenantType)
    {
        var query = _db.Tenants.AsQueryable();

        if (tenantType == "host")
        {
            return await query.OrderBy(t => t.Type).ThenBy(t => t.Name).ToListAsync();
        }

        if (tenantType == "bayi")
        {
            return await query
                .Where(t => t.Id == currentTenantId || t.ParentTenantId == currentTenantId)
                .OrderBy(t => t.Type).ThenBy(t => t.Name).ToListAsync();
        }

        // musteri - sadece kendi tenant'ını görebilir
        return await query.Where(t => t.Id == currentTenantId).ToListAsync();
    }

    public bool CanCreateType(string currentType, string targetType)
    {
        return currentType switch
        {
            "host" => targetType is "bayi" or "musteri",
            "bayi" => targetType == "musteri",
            _ => false
        };
    }

    public List<string> GetAllowedChildTypes(string currentType)
    {
        return currentType switch
        {
            "host" => ["bayi", "musteri"],
            "bayi" => ["musteri"],
            _ => []
        };
    }

    public async Task<Tenant?> GetByIdAsync(uint id)
    {
        return await _db.Tenants
            .Include(t => t.ParentTenant)
            .Include(t => t.Addresses)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<bool> CanAccessAsync(uint currentTenantId, string currentType, uint targetTenantId)
    {
        if (currentType == "host") return true;

        var target = await _db.Tenants.FindAsync(targetTenantId);
        if (target == null) return false;

        if (currentType == "bayi")
            return target.Id == currentTenantId || target.ParentTenantId == currentTenantId;

        return target.Id == currentTenantId;
    }

    public async Task<Tenant> CreateAsync(Tenant tenant)
    {
        tenant.CreatedAt = DateTime.UtcNow;
        tenant.UpdatedAt = DateTime.UtcNow;
        _db.Tenants.Add(tenant);
        await _db.SaveChangesAsync();
        return tenant;
    }

    public async Task UpdateAsync(Tenant tenant)
    {
        tenant.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }
}
