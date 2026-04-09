using Microsoft.AspNetCore.Mvc;
using StudySphere.Web.Models.Admin;
using StudySphere.Web.Services.Contracts;

namespace StudySphere.Web.Areas.Admin.Controllers;

public class CategoriesController : BaseAdminController
{
    private readonly IAdminDashboardService adminDashboardService;

    public CategoriesController(IAdminDashboardService adminDashboardService)
    {
        this.adminDashboardService = adminDashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var model = await this.adminDashboardService.GetCategoriesAsync();
        return this.View(model);
    }

    public IActionResult Create()
    {
        return this.View(new AdminCategoryInputModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(AdminCategoryInputModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        await this.adminDashboardService.CreateCategoryAsync(model);
        return this.RedirectToAction(nameof(this.Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var model = await this.adminDashboardService.GetCategoryAsync(id);
        if (model is null)
        {
            return this.RedirectToAction("StatusCodePage", "Error", new { area = "", statusCode = 404 });
        }

        return this.View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AdminCategoryInputModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        await this.adminDashboardService.UpdateCategoryAsync(model);
        return this.RedirectToAction(nameof(this.Index));
    }

    public IActionResult Delete(int id)
    {
        this.ViewBag.CategoryId = id;
        return this.View();
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        await this.adminDashboardService.DeleteCategoryAsync(id);
        return this.RedirectToAction(nameof(this.Index));
    }
}
