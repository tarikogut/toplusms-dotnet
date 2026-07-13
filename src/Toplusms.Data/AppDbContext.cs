using Microsoft.EntityFrameworkCore;
using Toplusms.Models;

namespace Toplusms.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<SMSMessage> SMSMessages => Set<SMSMessage>();
    public DbSet<SMSTemplate> SMSTemplates => Set<SMSTemplate>();
    public DbSet<SMSHeader> SMSHeaders => Set<SMSHeader>();
    public DbSet<Blacklist> Blacklists => Set<Blacklist>();
    public DbSet<LegalPaper> LegalPapers => Set<LegalPaper>();
    public DbSet<InfoPage> InfoPages => Set<InfoPage>();
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<PaymentNotification> PaymentNotifications => Set<PaymentNotification>();
    public DbSet<Originator> Originators => Set<Originator>();
    public DbSet<Campaign> Campaigns => Set<Campaign>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tenant
        modelBuilder.Entity<Tenant>(e =>
        {
            e.HasIndex(t => t.Domain).IsUnique();
            e.HasIndex(t => t.Code).IsUnique();
            e.HasMany(t => t.Children).WithOne(t => t.ParentTenant).HasForeignKey(t => t.ParentTenantId);
        });

        // User
        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => new { u.TenantId, u.Username }).IsUnique();
            e.HasOne(u => u.Tenant).WithMany(t => t.Users).HasForeignKey(u => u.TenantId);
            e.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
            e.HasOne(u => u.ParentUser).WithMany(u => u.Children).HasForeignKey(u => u.ParentUserId);
        });

        // Contact
        modelBuilder.Entity<Contact>(e =>
        {
            e.HasOne(c => c.Group).WithMany(g => g.Contacts).HasForeignKey(c => c.GroupId);
        });

        // Soft delete filter
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entity.ClrType))
            {
                var method = typeof(AppDbContext).GetMethod(nameof(SetSoftDeleteFilter))!
                    .MakeGenericMethod(entity.ClrType);
                method.Invoke(this, [modelBuilder]);
            }
        }
    }

    private static void SetSoftDeleteFilter<T>(ModelBuilder builder) where T : BaseEntity
    {
        builder.Entity<T>().HasQueryFilter(e => e.DeletedAt == null);
    }
}
