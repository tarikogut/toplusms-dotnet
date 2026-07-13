using Microsoft.EntityFrameworkCore;
using Toplusms.Models;

namespace Toplusms.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Tenants.AnyAsync()) return;

        // Roles
        var adminRole = new Role { Name = "admin" };
        var bayiRole = new Role { Name = "bayi" };
        var musteriRole = new Role { Name = "musteri" };
        db.Roles.AddRange(adminRole, bayiRole, musteriRole);
        await db.SaveChangesAsync();

        // Host tenant
        var hostTenant = new Tenant
        {
            Domain = "toplusms.link",
            Name = "Toplusms",
            Code = "HOST",
            Slug = "toplusms",
            Type = "host",
            Status = "active",
            TaxNumber = "1234567890",
            TaxOffice = "Kadıköy VD",
            CustomerType = "T",
            Phone = "08501234567",
            Email = "info@toplusms.link"
        };
        db.Tenants.Add(hostTenant);
        await db.SaveChangesAsync();

        // Admin user
        var adminUser = new User
        {
            TenantId = hostTenant.Id,
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Name = "Admin",
            Surname = "User",
            Email = "admin@toplusms.link",
            RoleId = adminRole.Id,
            Status = "active"
        };
        db.Users.Add(adminUser);
        await db.SaveChangesAsync();

        // Demo musteri user
        var musteriUser = new User
        {
            TenantId = hostTenant.Id,
            Username = "musteri",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Name = "Müşteri",
            Surname = "User",
            Email = "musteri@toplusms.link",
            RoleId = musteriRole.Id,
            Status = "active"
        };
        db.Users.Add(musteriUser);
        await db.SaveChangesAsync();

        // Demo headers
        db.SMSHeaders.AddRange(
            new SMSHeader { TenantId = hostTenant.Id, Name = "TOPLUSMS" },
            new SMSHeader { TenantId = hostTenant.Id, Name = "SMS PANEL" }
        );
        await db.SaveChangesAsync();

        // Demo bank accounts
        db.BankAccounts.Add(new BankAccount
        {
            TenantId = hostTenant.Id,
            BankName = "Ziraat Bankası",
            BranchCode = "1234",
            AccountNumber = "1234567890",
            Iban = "TR12 3456 7890 1234 5678 9012 34",
            HolderName = "Toplusms"
        });
        await db.SaveChangesAsync();
    }
}
