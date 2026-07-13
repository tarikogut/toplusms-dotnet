namespace Toplusms.Services;

public static class PermissionHelper
{
    public static bool HasPermission(string userRole, string resource, string action)
    {
        if (string.IsNullOrEmpty(userRole)) return false;
        if (userRole == "admin") return true;

        var rules = GetRolePermissions(userRole);
        return rules.Any(r =>
        {
            var resourceMatch = r.Resource == "*" || r.Resource == resource || resource.StartsWith(r.Resource.TrimEnd('*').TrimEnd('/'));
            var actionMatch = r.Action == "*" || r.Action == action;
            return resourceMatch && actionMatch;
        });
    }

    private static List<PermissionRule> GetRolePermissions(string role) => role switch
    {
        "bayi" => new List<PermissionRule>
        {
            new("/api/v1/me", "read"),
            new("/api/v1/dashboard/*", "read"),
            new("/api/v1/permissions", "read"),
            new("/api/v1/sms/*", "*"),
            new("/api/v1/phonebook/*", "*"),
            new("/api/v1/subscribers/*", "*"),
            new("/api/v1/account/*", "*"),
            new("/api/v1/stats/*", "read"),
            new("/api/v1/reseller/*", "*"),
        },
        "musteri" => new List<PermissionRule>
        {
            new("/api/v1/me", "read"),
            new("/api/v1/dashboard/*", "read"),
            new("/api/v1/permissions", "read"),
            new("/api/v1/sms/*", "*"),
            new("/api/v1/phonebook/*", "*"),
            new("/api/v1/account/profile", "*"),
            new("/api/v1/account/change_password", "write"),
            new("/api/v1/account/blacklist", "*"),
            new("/api/v1/info/*", "read"),
            new("/api/v1/sub-users/*", "*"),
        },
        _ => new List<PermissionRule>()
    };

    private record PermissionRule(string Resource, string Action);
}
