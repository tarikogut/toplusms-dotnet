using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Toplusms.Services;

namespace Toplusms.Web.Controllers;

public class LoginController : Controller
{
    private readonly AuthService _auth;

    public LoginController(AuthService auth)
    {
        _auth = auth;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Dashboard");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(string code, string username, string password)
    {
        // Resolve tenant by code
        var tenant = await _auth.ResolveTenantAsync(code);
        if (tenant == null)
        {
            ModelState.AddModelError("", "Geçersiz tenant kodu");
            return View();
        }

        var user = await _auth.AuthenticateAsync(tenant.Id, username, password);
        if (user == null)
        {
            ModelState.AddModelError("", "Invalid credentials");
            return View();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new("TenantId", user.TenantId.ToString()),
            new("TenantType", tenant.Type),
            new("TenantCode", tenant.Code),
            new(ClaimTypes.Role, user.Role?.Name ?? ""),
            new("FullName", $"{user.Name} {user.Surname}".Trim()),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTime.UtcNow.AddDays(1)
        });

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index");
    }
}
