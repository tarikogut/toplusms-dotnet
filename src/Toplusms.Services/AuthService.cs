using Microsoft.EntityFrameworkCore;
using Toplusms.Data;
using Toplusms.Models;

namespace Toplusms.Services;

public class AuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User?> AuthenticateAsync(uint tenantId, string username, string password)
    {
        var user = await _db.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.TenantId == tenantId && u.Username == username && u.Status == "active");

        if (user == null) return null;
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null;

        return user;
    }

    public async Task<Tenant?> ResolveTenantAsync(string? host)
    {
        if (string.IsNullOrEmpty(host)) return null;
        return await _db.Tenants.FirstOrDefaultAsync(t => t.Domain == host && t.Status == "active");
    }

    public async Task<User?> GetByIdAsync(uint id)
    {
        return await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
    }
}
