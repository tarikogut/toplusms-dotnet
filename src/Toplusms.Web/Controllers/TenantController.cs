using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Toplusms.Models;
using Toplusms.Services;

namespace Toplusms.Web.Controllers;

[Authorize]
public class TenantController : Controller
{
    private readonly TenantService _tenantService;
    private readonly AuthService _auth;

    public TenantController(TenantService tenantService, AuthService auth)
    {
        _tenantService = tenantService;
        _auth = auth;
    }

    private uint CurrentTenantId => uint.Parse(User.FindFirst("TenantId")!.Value);
    private string TenantType => User.FindFirst("TenantType")!.Value;
    private string UserRole => User.FindFirst(ClaimTypes.Role)?.Value ?? "";

    private bool CanManageTenants => TenantType != "musteri" && UserRole != "musteri";

    public async Task<IActionResult> Index()
    {
        if (!CanManageTenants)
        {
            TempData["Error"] = "Bu sayfaya erişim yetkiniz yok.";
            return RedirectToAction("Index", "Dashboard");
        }

        var tenants = await _tenantService.GetVisibleTenantsAsync(CurrentTenantId, TenantType);
        return View(tenants);
    }

    [HttpGet]
    public IActionResult Create()
    {
        if (!CanManageTenants) return Forbid();

        var allowedTypes = _tenantService.GetAllowedChildTypes(TenantType);
        if (allowedTypes.Count == 0) return Forbid();

        ViewBag.AllowedTypes = allowedTypes;
        return View(new Tenant { Type = allowedTypes[0] });
    }

    [HttpPost]
    public async Task<IActionResult> Create(Tenant model)
    {
        if (!_tenantService.CanCreateType(TenantType, model.Type))
            return Forbid();

        model.ParentTenantId = CurrentTenantId;
        model.Status = "active";
        model.Domain = string.IsNullOrEmpty(model.Domain) ? $"{model.Code?.ToLower()}.toplusms.link" : model.Domain;

        await _tenantService.CreateAsync(model);

        TempData["Success"] = $"Tenant '{model.Name}' oluşturuldu.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(uint id)
    {
        if (!await _tenantService.CanAccessAsync(CurrentTenantId, TenantType, id))
            return Forbid();

        var tenant = await _tenantService.GetByIdAsync(id);
        if (tenant == null) return NotFound();

        return View(tenant);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Tenant model)
    {
        var tenant = await _tenantService.GetByIdAsync(model.Id);
        if (tenant == null) return NotFound();

        if (!await _tenantService.CanAccessAsync(CurrentTenantId, TenantType, tenant.Id))
            return Forbid();

        tenant.Name = model.Name;
        tenant.Code = model.Code;
        tenant.Phone = model.Phone;
        tenant.Email = model.Email;
        tenant.TaxNumber = model.TaxNumber;
        tenant.TaxOffice = model.TaxOffice;
        tenant.IdentityNumber = model.IdentityNumber;
        tenant.CustomerType = model.CustomerType;
        tenant.Status = model.Status;
        tenant.Domain = model.Domain;

        await _tenantService.UpdateAsync(tenant);

        TempData["Success"] = $"Tenant '{tenant.Name}' güncellendi.";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Details(uint id)
    {
        if (!await _tenantService.CanAccessAsync(CurrentTenantId, TenantType, id))
            return Forbid();

        var tenant = await _tenantService.GetByIdAsync(id);
        if (tenant == null) return NotFound();

        return View(tenant);
    }
}
